using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.IO;
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
                            var spawnPosition = GetClosestEmptyCellToDesired(spawnerComp.SpawnTarget);
                            posComp.X = (int) spawnPosition.X * GlobalConfig.TileSize;
                            posComp.Y = (int) spawnPosition.Y * GlobalConfig.TileSize;
                        }
                    }
                }
            }
        }

        // NOTE: Doesn't exactly work as expected, but it will do the job
        private Vector2 GetClosestEmptyCellToDesired(Vector2 spawnTarget)
        {
            // If desired cell is empty, return it
            if (_gameMap.Entities[(int) spawnTarget.X, (int) spawnTarget.Y] == null)
            {
                return spawnTarget;
            }

            // Look for closest empty cell in spiral
            int[] xRot = new int[] { 1, 1, 0, -1, -1, -1, 0, 1 };
            int[] yRot = new int[] { 0, -1, -1, -1, 0, 1, 1, 1 };
            int i = 0;
            int distance = 1;
            while (_gameMap.Entities[(int) spawnTarget.X + distance * xRot[i], (int) spawnTarget.Y + distance * yRot[i]] != null)
            {
                i++;
                if (i >= xRot.Length)
                {
                    i = 0;
                    distance++;
                }
            }

            return new Vector2((int)spawnTarget.X + distance * xRot[i], (int)spawnTarget.Y + distance * yRot[i]);
        }
    }
}
