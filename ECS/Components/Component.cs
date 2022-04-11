using System;

namespace Revolution.ECS.Components
{
    public abstract class Component
    {
        public virtual Type ComponentType()
        {
            var type = GetType();
            return type;
        }

        public virtual void CleanUp()
        {

        }
    }
}