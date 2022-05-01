using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Revolution.ECS.Entities
{
    public class EntityManager
    {
        // Alive entities   
        private static IDictionary<int, Entity> _entities = new ConcurrentDictionary<int, Entity>();

        public static T CreateEntity<T>() where T : Entity
        {
            // Create entity instance from generic type
            var obj = Activator.CreateInstance<T>();
           
            RegisterEntity(obj);

            return obj;
        }

        public static Entity CreateEntity(Type type)
        {
            var entity = Activator.CreateInstance(type);
            if (entity is Entity)
            {
                RegisterEntity(entity as Entity);
                return entity as Entity;
            }

            return null;
        }

        private static void RegisterEntity(Entity e)
        {
            // Subscribe to destroy event in order to properly clean up after disposing entity
            e.DestroyEvent += OnEntityDestroy;

            // Add instance to active entities
            _entities[e.Id] = e;
        }

        public static T GetEntity<T>(int id) where T : Entity
        {
            return _entities[id] as T;
        }

        public static IEnumerable<Entity> GetEntities()
        {
            return _entities.Values;
        }

        private static void OnEntityDestroy(object? sender, Entity destroyable)
        {
            // Remove from active entities 
            destroyable.DestroyEvent -= OnEntityDestroy;
            _entities.Remove(destroyable.Id);
        }
    }
}