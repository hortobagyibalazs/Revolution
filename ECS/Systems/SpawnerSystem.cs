using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using Revolution.Commands;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.HUD.Events;
using Revolution.IO;
using Revolution.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.ECS.Systems
{
    public class SpawnerSystem : ISystem, IRecipient<UnitPurchaseCommand>
    {
        private MapData _gameMap;
        private IMessenger _messenger = Ioc.Default.GetService<IMessenger>();

        public SpawnerSystem(MapData gameMap)
        {
            _gameMap = gameMap;

            _messenger.Register(this);
        }

        public void Receive(UnitPurchaseCommand message)
        {
            var entity = EntityManager.CreateEntity(message.UnitType);
            var player = PlayerHelper.GetGuiControlledPlayer();
            var playerResources = player.GetComponent<ResourceComponent>();

            var unitPrice = entity.GetComponent<PriceComponent>();
            var buildingSpawner = message.Building.GetComponent<SpawnerComponent>();
            if ((unitPrice == null || unitPrice.Buy(playerResources)) && buildingSpawner != null)
            {
                buildingSpawner.SpawnQueue.Enqueue(message.UnitType);
            }
            else
            {
                _messenger.Send(new ShowToastEvent(GlobalStrings.NotEnoughResources));
            }
            entity.Destroy();
        }

        public void Update(int deltaMs)
        {
            foreach(var entity in EntityManager.GetEntities())
            {
                var spawnerComp = entity.GetComponent<SpawnerComponent>();
                if (spawnerComp != null)
                {
                    Type entityType = null;
                    spawnerComp.SpawnQueue.TryDequeue(out entityType);

                    if (entityType != null)
                    {
                        var newEntity = EntityManager.CreateEntity(entityType);
                        var gmoComp = newEntity.GetComponent<GameMapObjectComponent>();
                        var posComp = newEntity.GetComponent<PositionComponent>();
                        if (gmoComp != null && posComp != null)
                        {
                            var spawnPosition = MapHelper.GetClosestEmptyCellToDesired(spawnerComp.SpawnTarget, _gameMap, entity);
                            if (spawnPosition != null)
                            {
                                posComp.X = (int)spawnPosition?.X * GlobalConfig.TileSize;
                                posComp.Y = (int)spawnPosition?.Y * GlobalConfig.TileSize;
                            }
                        }
                    }
                }
            }
        }
    }
}
