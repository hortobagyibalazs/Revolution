using System;
using System.Collections.Generic;
using Component = Revolution.ECS.Components.Component;

namespace Revolution.ECS.Entities
{
    public abstract class Entity
    {
        private static int _idCounter;
        private IDictionary<Type, Component> components;

        public int Id { get; }
        public event EventHandler<Entity> DestroyEvent;

        protected Entity()
        {
            Id = _idCounter++;
            components = new Dictionary<Type, Component>();
        }

        public virtual void Destroy()
        {
            DestroyEvent?.Invoke(this, this);
        }

        public T? GetComponent<T>() where T : Component
        {
            foreach (var component in components)
            {
                if (component.Key.Equals(typeof(T)))
                {
                    return component.Value as T;
                }
            }

            return null;
        }

        public IEnumerable<Component> GetComponents()
        {
            return components.Values;
        }

        protected void AddComponent(Component component)
        {
            var type = component.GetType();
            if (!components.ContainsKey(type))
            {
                components[type] = component;
            }
            else
            {
                // TODO : Throw exception or something
            }
        }

        protected void RemoveComponent(Component component)
        {
            var type = component.GetType();
            components.Remove(type);
        }
    }
}