using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.IO;
using System;
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
                if (movementComp != null && gameObjectComp != null)
                {
                    if (movementComp.CurrentTarget != null)
                    {
                        if (((int) movementComp.CurrentTarget?.X == gameObjectComp.X) 
                            && ((int) movementComp.CurrentTarget?.Y == gameObjectComp.Y))
                        {
                            SetNextDestination(movementComp, gameObjectComp);
                        }
                    }
                    else
                    {
                        SetNextDestination(movementComp, gameObjectComp);
                    }

                    var collisionComp = entity.GetComponent<CollisionComponent>();
                    bool collides = false;
                    if (collisionComp != null)
                    {
                        collides = EntityCollides(entity);
                    }

                    if (!collides)
                    {
                        var posComp = entity.GetComponent<PositionComponent>();
                        posComp.X += movementComp.VelocityX;
                        posComp.Y += movementComp.VelocityY;
                    } 
                    else
                    {
                        movementComp.Stop();
                    }
                }
            }
        }

        private void SetNextDestination(MovementComponent movementComp, GameMapObjectComponent gameObjectComp)
        {
            Vector2 nextDest;
            if (movementComp.Path.TryDequeue(out nextDest))
            {
                movementComp.CurrentTarget = nextDest;
                SetVelocity(nextDest, movementComp, gameObjectComp);
            }
            else
            {
                movementComp.Stop();
            }
        }

        private void SetVelocity(Vector2 nextDest, MovementComponent movementComp, GameMapObjectComponent gameObjectComp)
        {
            if (nextDest != null)
            {
                var currentX = gameObjectComp.X;
                var currentY = gameObjectComp.Y;

                var targetX = nextDest.X;
                var targetY = nextDest.Y;

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