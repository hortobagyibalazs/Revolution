using System;
using Avalonia.Controls;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.IO;

namespace Revolution.ECS.Systems
{
    public class BuildingSystem : ISystem
    {
        private ScrollViewer ScrollViewer;

        public BuildingSystem(ScrollViewer scrollViewer)
        {
            ScrollViewer = scrollViewer;
        }
        
        public void Update(int deltaMs)
        {
            foreach (var entity in EntityManager.GetEntities())
            {
                var buildingComponent = entity.GetComponent<BuildingComponent>();
                var mapObjectComponent = entity.GetComponent<GameMapObjectComponent>();
                if (mapObjectComponent != null && buildingComponent?.State == BuildingState.Placing)
                {
                    int tileX = (int) (Mouse.Instance.CursorX + ScrollViewer.Offset.X) / GlobalConfig.TileSize;
                    int tileY = (int) (Mouse.Instance.CursorY + ScrollViewer.Offset.Y) / GlobalConfig.TileSize;

                    // TODO : Fix clipping bug caused by moving the cursor at the very right or very bottom of the map 
                    mapObjectComponent.X = Math.Max(tileX - 1, 0);
                    mapObjectComponent.Y = Math.Max(tileY - 1, 0);
                }
            }
        }
    }
}