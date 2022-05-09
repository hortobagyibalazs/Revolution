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
using Revolution.StateMachines.Build;

namespace Revolution.ECS.Systems
{
    public class BuildingSystem : ISystem, IRecipient<BuildingPurchaseCommand>, IRecipient<BuildWithPeasantCommand>
    {
        private IMessenger _messenger = Ioc.Default.GetService<IMessenger>();

        private ScrollViewer ScrollViewer;
        private Canvas Canvas;

        public BuildingSystem(ScrollViewer scrollViewer, Canvas canvas)
        {
            ScrollViewer = scrollViewer;
            Canvas = canvas;

            _messenger.Register<BuildingPurchaseCommand>(this);
            _messenger.Register<BuildWithPeasantCommand>(this);
        }

        public void Receive(BuildingPurchaseCommand message)
        {
            var entity = EntityManager.CreateEntity(message.BuildingType);
            var player = PlayerHelper.GetGuiControlledPlayer();
            var playerResources = player.GetComponent<ResourceComponent>();

            var buildingComponent = entity.GetComponent<BuildingComponent>();
            var buildingPrice = entity.GetComponent<PriceComponent>();
            if (buildingComponent != null && buildingPrice.Buy(playerResources))
            {
                buildingComponent.State = BuildingState.Placing;
            }
            else
            {
                _messenger.Send(new ShowToastEvent(GlobalStrings.NotEnoughResources));
                entity.Destroy();
            }
        }

        public void Receive(BuildWithPeasantCommand message)
        {
            message.Peasant.GetComponent<StateMachineComponent>().StateMachine.CurrentState
                = new MoveToConstructionSiteState(message.Peasant, message.Building);
        }

        public void Update(int deltaMs)
        {
            foreach (var entity in EntityManager.GetEntities())
            {
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
                        _messenger.Send(new BuildWithPeasantCommand(entity));
                    }

                    return;
                }
            }
        }
    }
}