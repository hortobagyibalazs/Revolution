using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.IO;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Windows.Input;

namespace Revolution.ECS.Systems
{
    public class MovementSystem : ISystem
    {
        public void Update(int deltaMs)
        {
            foreach (var entity in EntityManager.GetEntities())
            {
                var movementComp = entity.GetComponent<MovementComponent>();
                var gameObjectComp = entity.GetComponent<GameMapObjectComponent>();
                var directionComp = entity.GetComponent<DirectionComponent>();
                var posComp = entity.GetComponent<PositionComponent>();

                if (movementComp != null && gameObjectComp != null && posComp != null)
                {
                    if (movementComp.CurrentTarget != null)
                    {
                        int targetX = ((int) movementComp.CurrentTarget?.X) * GlobalConfig.TileSize;
                        int targetY = ((int) movementComp.CurrentTarget?.Y) * GlobalConfig.TileSize;
                        if ((posComp.X == targetX) && (posComp.Y == targetY))
                        {
                            SetNextDestination(movementComp, posComp, directionComp);
                        }

                    }
                    else
                    {
                        SetNextDestination(movementComp, posComp, directionComp);
                        continue;
                    }

                    posComp.X += movementComp.VelocityX;
                    posComp.Y += movementComp.VelocityY;
                }
            }
        }

        private void SetNextDestination(MovementComponent movementComp, PositionComponent posComp, DirectionComponent directionComp)
        {
            Vector2 nextDest;
            if (movementComp.Path.TryDequeue(out nextDest))
            {
                movementComp.CurrentTarget = nextDest;
                SetVelocity(nextDest, movementComp, posComp, directionComp);
            }
            else
            {
                movementComp.Stop();
            }
        }

        private void SetVelocity(Vector2 nextDest, MovementComponent movementComp, PositionComponent posComp, DirectionComponent directionComp)
        {
            if (nextDest != null)
            {
                var currentX = posComp.X;
                var currentY = posComp.Y;

                var targetX = (int) nextDest.X * GlobalConfig.TileSize;
                var targetY = (int) nextDest.Y * GlobalConfig.TileSize;

                if (currentX < targetX)
                {
                    movementComp.VelocityX = movementComp.MaxVelocity;
                } 
                else if (currentX > targetX)
                {
                    movementComp.VelocityX = -movementComp.MaxVelocity;
                }
                else
                {
                    movementComp.VelocityX = 0;
                }

                if (currentY < targetY)
                {
                    movementComp.VelocityY = movementComp.MaxVelocity;
                }
                else if (currentY > targetY)
                {
                    movementComp.VelocityY = -movementComp.MaxVelocity;
                }
                else
                {
                    movementComp.VelocityY = 0;
                }

                if (directionComp != null)
                {
                    if (movementComp.VelocityX > 0)
                    {
                        directionComp.Direction = Direction.Right;
                    }
                    else if (movementComp.VelocityX < 0)
                    {
                        directionComp.Direction = Direction.Left;
                    }
                }
            }
        }

        private bool EntityCollides(Entity entity)
        {
            var collisionComp = entity.GetComponent<CollisionComponent>();
            foreach (var entity2 in EntityManager.GetEntities())
            {
                var collisionComp2 = entity2.GetComponent<CollisionComponent>();
                if (entity2 != entity && collisionComp2 != null &&
                    collisionComp.CollidesWith(collisionComp2))
                {
                    return true;
                }
            }

            return false;
        }
    }
}