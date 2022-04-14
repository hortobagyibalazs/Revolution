using System;
using System.Collections.Generic;
using System.ComponentModel;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.IO;

namespace Revolution.ECS.Systems
{
    public class MapSystem : ISystem
    {
        private HashSet<Entity> cachedEntities;
        private IDictionary<GameMapObjectComponent, Entity> componentEntityPairs;
        private MapData mapData;

        public MapSystem(MapData map)
        {
            cachedEntities = new HashSet<Entity>();
            componentEntityPairs = new Dictionary<GameMapObjectComponent, Entity>();
            mapData = map;
        }

        public void Update(int deltaMs)
        {
            foreach (var entity in EntityManager.GetEntities())
            {
                if (entity is Tile) continue;

                var mapObjectComp = entity.GetComponent<GameMapObjectComponent>();
                
                if (mapObjectComp != null && !cachedEntities.Contains(entity))
                {
                    FillMapArea(entity, mapObjectComp.X, mapObjectComp.Y, mapObjectComp.Width, mapObjectComp.Height);
                    cachedEntities.Add(entity);
                    mapObjectComp.PropertyChanged += OnMapObjectAttributeChanged;
                    componentEntityPairs[mapObjectComp] = entity;
                    entity.DestroyEvent += RemoveEntity;
                }
            }
        }

        private void FillMapArea(Entity entity, int x, int y, int width, int height)
        {
            if (width == 0 || height == 0) return;

            for (int _x = x; _x < x + width; _x++)
            {
                for (int _y = y; _y < y + height; _y++)
                {
                    mapData.Entities[_x, _y] = entity;
                }
            }
        }

        private void RemoveEntity(object? sender, Entity e)
        {
            var mapObjectComp = e.GetComponent<GameMapObjectComponent>();
            if (mapObjectComp != null)
            {
                mapObjectComp.PropertyChanged -= OnMapObjectAttributeChanged;
                FillMapArea(null, mapObjectComp.X, mapObjectComp.Y, mapObjectComp.Width, mapObjectComp.Height);
                componentEntityPairs[mapObjectComp] = null;
            }

            e.DestroyEvent -= RemoveEntity;
            cachedEntities.Remove(e);
        }

        private void OnMapObjectAttributeChanged(object? sender, PropertyChangedEventArgs e)
        {
            var eventArgs = e as PropertyChangedEventGameMapObjectArgs;
            var senderComp = sender as GameMapObjectComponent;
            FillMapArea(null, eventArgs.OldX, eventArgs.OldY, eventArgs.OldWidth, eventArgs.OldHeight);
            FillMapArea(componentEntityPairs[senderComp], eventArgs.NewX, eventArgs.NewY, eventArgs.NewWidth, eventArgs.NewHeight);
        }
    }
}