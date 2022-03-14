using System;
using Revolution.ECS.Components;

namespace Revolution.ECS.Entities
{
    public abstract class Entity
    {
        private static int _idCounter;
        public int Id { get; }
        public event EventHandler<Entity> DestroyEvent;

        protected Entity()
        {
            Id = _idCounter++;
        }
        
        public abstract void Update(int deltaMs);

        public virtual void Destroy()
        {
            DestroyEvent?.Invoke(this, this);
        }
    }
}