using Revolution.ECS.Components;
using Revolution.ECS.Entities;
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
    public class SpawnerSystem : ISystem
    {
        private MapData _gameMap;

        public SpawnerSystem(MapData gameMap)
        {
            _gameMap = gameMap;
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
                            var spawnPosition = MapHelper.GetClosestEmptyCellToDesired(spawnerComp.SpawnTarget, _gameMap);
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
