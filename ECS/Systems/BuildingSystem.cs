using System;
using System.Numerics;
using System.Windows;
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

        private MapData _gameMap;

        public BuildingSystem(ScrollViewer scrollViewer, Canvas canvas, MapData map)
        {
            ScrollViewer = scrollViewer;
            Canvas = canvas;

            _gameMap = map;

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

                    if (Mouse.LeftButton == MouseButtonState.Pressed /*&& CanPlaceBuilding(entity)*/)
                    {
                        buildingComponent.State = BuildingState.UnderConstruction;
                        _messenger.Send(new BuildWithPeasantCommand(entity));
                    }

                    return;
                }
            }
        }

        private bool CanPlaceBuilding(Entity entity)
        {
            var mapObjectComponent = entity.GetComponent<GameMapObjectComponent>();
            int startX = mapObjectComponent.X;
            int startY = mapObjectComponent.Y;
            int endX = mapObjectComponent.X + mapObjectComponent.Width - 1;
            int endY = mapObjectComponent.Y + mapObjectComponent.Height - 1;
            
            // check for colliding tiles
            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    if (!_gameMap.Tiles[x, y].TrueForAll(tile => !tile.Colliding))
                    {
                        return false;
                    }
                }
            }

            // check for entities (can't use MapData for this)
            foreach (var _e in EntityManager.GetEntities())
            {
                if (_e != entity)
                {
                    var otherMapObjectComp = _e.GetComponent<GameMapObjectComponent>();
                    if (otherMapObjectComp != null)
                    {
                        bool intersects = new Rect(
                            mapObjectComponent.X, mapObjectComponent.Y,
                            mapObjectComponent.Width, mapObjectComponent.Height
                            ).IntersectsWith(new Rect(
                                otherMapObjectComp.X, otherMapObjectComp.Y,
                                otherMapObjectComp.Width, otherMapObjectComp.Height
                                ));

                        if (intersects)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}