using System;
using System.Windows.Controls;
using System.Windows.Input;
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
            if (Keyboard.IsKeyDown(Key.B))
            {
                var entity = EntityManager.CreateEntity<House>();
                entity.GetComponent<BuildingComponent>().State = BuildingState.Placing;
                entity.GetComponent<GameMapObjectComponent>().X = GetGameObjectPosBasedOnCursorX();
                entity.GetComponent<GameMapObjectComponent>().Y = GetGameObjectPosBasedOnCursorY();
            } 
            else if (Keyboard.IsKeyDown(Key.N))
            {
                var entity = EntityManager.CreateEntity<TownCenter>();
                entity.GetComponent<BuildingComponent>().State = BuildingState.Placing;
                entity.GetComponent<GameMapObjectComponent>().X = GetGameObjectPosBasedOnCursorX();
                entity.GetComponent<GameMapObjectComponent>().Y = GetGameObjectPosBasedOnCursorY();
            }
        }

        private int GetGameObjectPosBasedOnCursorX()
        {
            return (int) (Mouse.GetPosition(ScrollViewer).X + ScrollViewer.HorizontalOffset) / GlobalConfig.TileSize;
        }

        private int GetGameObjectPosBasedOnCursorY()
        {
            return (int) (Mouse.GetPosition(ScrollViewer).Y + ScrollViewer.VerticalOffset) / GlobalConfig.TileSize;
        }
    }
}