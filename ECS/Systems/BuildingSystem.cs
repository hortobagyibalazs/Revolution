using System;
using System.Numerics;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using Revolution.Commands;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.HUD.Events;
using Revolution.IO;

namespace Revolution.ECS.Systems
{
    public class BuildingSystem : ISystem, IRecipient<BuildingPurchaseEvent>
    {
        private IMessenger _messenger = Ioc.Default.GetService<IMessenger>();

        private ScrollViewer ScrollViewer;
        private Canvas Canvas;

        public BuildingSystem(ScrollViewer scrollViewer, Canvas canvas)
        {
            ScrollViewer = scrollViewer;
            Canvas = canvas;

            _messenger.Register<BuildingPurchaseEvent>(this);
        }

        public void Receive(BuildingPurchaseEvent message)
        {
            var entity = EntityManager.CreateEntity(message.BuildingType);
            var buildingComponent = entity.GetComponent<BuildingComponent>();
            if (buildingComponent != null)
            {
                buildingComponent.State = BuildingState.Placing;
            }
        }

        public void Update(int deltaMs)
        {
            foreach (var entity in EntityManager.GetEntities())
            {
                var movementComponent = entity.GetComponent<MovementComponent>();
                var buildingComponent = entity.GetComponent<BuildingComponent>();
                var mapObjectComponent = entity.GetComponent<GameMapObjectComponent>();
                if (mapObjectComponent != null && buildingComponent?.State == BuildingState.Placing)
                {
                    int tileX = GetGameObjectPosBasedOnCursorX();
                    int tileY = GetGameObjectPosBasedOnCursorY();

                    // TODO : Fix clipping bug caused by moving the cursor at the very right or very bottom of the map 
                    mapObjectComponent.X = Math.Max(tileX - 1, 0);
                    mapObjectComponent.Y = Math.Max(tileY - 1, 0);

                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                    {
                        foreach (var entity2 in EntityManager.GetEntities())
                        {
                            if (entity != entity2)
                            {
                                var collisionComp = entity2.GetComponent<CollisionComponent>();
                                if (collisionComp != null && collisionComp.CollidesWith(entity.GetComponent<CollisionComponent>()))
                                {
                                    return;
                                }
                            }
                        }
                        buildingComponent.State = BuildingState.UnderConstruction;
                    }

                    return;
                }
            }

            // This is for testing
            if (Keyboard.IsKeyDown (Key.V))
            {
                var villager = EntityManager.CreateEntity<Villager>();
                var posComp = villager.GetComponent<PositionComponent>();
                var gmoComp = villager.GetComponent<GameMapObjectComponent>();

                posComp.X = GetGameObjectPosBasedOnCursorX() * GlobalConfig.TileSize;
                posComp.Y = GetGameObjectPosBasedOnCursorY() * GlobalConfig.TileSize;
                int startX = gmoComp.X - 1;
                int startY = gmoComp.Y - 1;
            }

            if (Mouse.LeftButton == MouseButtonState.Pressed && Canvas.IsMouseOver)
            {
                foreach (var entity in Entities.EntityManager.GetEntities())
                {
                    if (entity is Entities.Villager)
                    {
                        var movementComp = entity.GetComponent<MovementComponent>();
                        var dest = new Vector2(GetGameObjectPosBasedOnCursorX(), GetGameObjectPosBasedOnCursorY());
                        _messenger.Send(new FindRouteCommand(entity, dest));
                    }
                }
            }
        }

        private int GetGameObjectPosBasedOnCursorX()
        {
            return (int)(Mouse.GetPosition(ScrollViewer).X + ScrollViewer.HorizontalOffset) / GlobalConfig.TileSize;
        }

        private int GetGameObjectPosBasedOnCursorY()
        {
            return (int)(Mouse.GetPosition(ScrollViewer).Y + ScrollViewer.VerticalOffset) / GlobalConfig.TileSize;
        }
    }
}