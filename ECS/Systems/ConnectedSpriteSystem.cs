using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.ECS.Systems
{
    public class ConnectedSpriteSystem : ISystem
    {
        private MapData _gameMap;
        public ConnectedSpriteSystem(MapData map)
        {
            _gameMap = map;

            UpdateAllSprites();
            // Also subscribe to entity deaths so we can refresh connections
        }

        public void Update(int deltaMs)
        {
            
        }

        private void UpdateAllSprites()
        {
            var entities = EntityManager.GetEntities();
            foreach (var entity in entities)
            {
                var renderComp = entity.GetComponent<ConnectedSpriteComponent>();
                var gameMapObjectComp = entity.GetComponent<GameMapObjectComponent>();
                if (renderComp != null && gameMapObjectComp != null)
                {
                    SetConnectionsForSpriteComponent(gameMapObjectComp, renderComp);
                }
            }
        }

        private void SetConnectionsForSpriteComponent(
            GameMapObjectComponent mapObjectComp, 
            ConnectedSpriteComponent spriteComp)
        {
            int x = mapObjectComp.X;
            int y = mapObjectComp.Y;

            int minX = 0;
            int minY = 0;
            int maxX = (int) _gameMap.Dimension.X - 1;
            int maxY = (int) _gameMap.Dimension.Y - 1;

            string connectionString = "";
            for (int _x = x - 1; _x <= x + 1; _x++)
            {
                for (int y_ = y - 1; y_ <= y + 1; y_++)
                {
                    if (!(_x == x && y_ == y))
                    {
                        var otherSpriteComp = _gameMap.Entities[_x, y_]?.GetComponent<ConnectedSpriteComponent>();
                        int val = 1;
                        if (_x < minX || _x > maxX || y_ < minY || y_ > maxY 
                            || otherSpriteComp == null || otherSpriteComp.Id != spriteComp.Id)
                        {
                            val = 0;
                        }

                        connectionString += val.ToString();
                    }
                }
            }
 
            connectionString = connectionString.Substring(1) + connectionString[0];
            spriteComp.SetConnections(connectionString);
        }
    }
}
