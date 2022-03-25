using System.ComponentModel;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.IO;

namespace Revolution.ECS.Systems
{
    public class MapSystem : ISystem
    {
        public void Update(int deltaMs)
        {
            foreach (var entity in EntityManager.GetEntities())
            {
                var mapObjectComp = entity.GetComponent<GameMapObjectComponent>();
                
                if (mapObjectComp != null)
                {
                    mapObjectComp.PropertyChanged += OnMapObjectAttributeChanged;
                    entity.DestroyEvent += Unsubscribe;
                }
            }
        }

        private void Unsubscribe(object? sender, Entity e)
        {
            var mapObjectComp = e.GetComponent<GameMapObjectComponent>();
            if (mapObjectComp != null)
            {
                mapObjectComp.PropertyChanged -= OnMapObjectAttributeChanged;
            }

            e.DestroyEvent -= Unsubscribe;
        }

        private void OnMapObjectAttributeChanged(object? sender, PropertyChangedEventArgs e)
        {
            var eventArgs = e as PropertyChangedEventExtendedArgs;
            var mapObject = sender as GameMapObjectComponent;
        }
    }
}