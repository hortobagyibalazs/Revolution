using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using Revolution.Commands;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.IO;
using Revolution.Misc;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Windows.Input;

namespace Revolution.ECS.Systems
{
    public class MovementSystem : ISystem
    {
        private IMessenger _messenger = Ioc.Default.GetService<IMessenger>();
        private MapData _gameMap;

        public MovementSystem(MapData map)
        {
            _gameMap = map;
        }

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
                            SetNextDestination(entity, movementComp, posComp, directionComp);
                            continue;
                        }
                    }
                    else
                    {
                        SetNextDestination(entity, movementComp, posComp, directionComp);
                        continue;
                    }

                    posComp.X += movementComp.VelocityX;
                    posComp.Y += movementComp.VelocityY;
                }
            }
        }

        private void SetNextDestination(Entity entity, MovementComponent movementComp, PositionComponent posComp, DirectionComponent directionComp)
        {
            Vector2 nextDest;
            if (movementComp.Path.TryDequeue(out nextDest))
            {
                if (CellEmpty(nextDest, entity) && !IsOtherEntityMovingToCell(nextDest, entity))
                {
                    movementComp.CurrentTarget = nextDest;
                    SetVelocity(nextDest, movementComp, posComp);
                    SetDirection(directionComp, movementComp);
                } 
                else if (movementComp.Path.Count > 0)
                {
                    Vector2[] dests = new Vector2[movementComp.Path.Count];
                    movementComp.Path.CopyTo(dests, 0);
                    Vector2 finalDest = dests[dests.Length - 1];
                    ReplanRoute(finalDest, entity);
                }
            }
            else
            {
                movementComp.Stop();
            }
        }

        private void SetVelocity(Vector2 nextDest, MovementComponent movementComp, PositionComponent posComp)
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
            }
        }

        private void SetDirection(DirectionComponent directionComp, MovementComponent movementComp)
        {
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

        private void ReplanRoute(Vector2 nextDest, Entity entity)
        {
            var closestCell = MapHelper.GetClosestEmptyCellToDesired(nextDest, _gameMap, entity);
            if (closestCell != null)
            {
                _messenger.Send(new FindRouteCommand(entity, (Vector2)closestCell));
            }
        }

        private bool CellEmpty(Vector2 nextDest, Entity entity)
        {
            int x = (int)nextDest.X;
            int y = (int)nextDest.Y;

            var cell = _gameMap.Entities[x, y];
            return (cell == null || cell == entity) && _gameMap.Tiles[x, y].TrueForAll(tile => !tile.Colliding);
        }

        private bool IsOtherEntityMovingToCell(Vector2 cell, Entity entity)
        {
            foreach(var otherEntity in EntityManager.GetEntities())
            {
                if (entity == otherEntity) continue;
                
                var movementComp = otherEntity.GetComponent<MovementComponent>();
                if (movementComp != null)
                {
                    if (movementComp.CurrentTarget == cell)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}