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
using Revolution.Misc;

namespace Revolution.ECS.Systems
{
    public class BuildingSystem : ISystem, IRecipient<BuildingPurchaseCommand>
    {
        private IMessenger _messenger = Ioc.Default.GetService<IMessenger>();

        private ScrollViewer ScrollViewer;
        private Canvas Canvas;

        public BuildingSystem(ScrollViewer scrollViewer, Canvas canvas)
        {
            ScrollViewer = scrollViewer;
            Canvas = canvas;

            _messenger.Register<BuildingPurchaseCommand>(this);
        }

        public void Receive(BuildingPurchaseCommand message)
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
                    int tileX = MapHelper.GetGameObjectPosBasedOnCursorX(ScrollViewer);
                    int tileY = MapHelper.GetGameObjectPosBasedOnCursorY(ScrollViewer);

                    // TODO : Fix clipping bug caused by moving the cursor at the very right or very bottom of the map 
                    mapObjectComponent.X = Math.Max(tileX - 1, 0);
                    mapObjectComponent.Y = Math.Max(tileY - 1, 0);

                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                    {
                        buildingComponent.State = BuildingState.UnderConstruction;
                    }

                    return;
                }
            }
        }
    }
}