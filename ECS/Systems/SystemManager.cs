using System.Collections.Generic;

namespace Revolution.ECS.Systems
{
    public class SystemManager
    {
        private HashSet<ISystem> systems = new();
        
        public void RegisterSystem(ISystem system)
        {
            systems.Add(system);
        }
        
        public void Update(int deltaMs)
        {
            foreach (var system in systems)
            {
                system.Update(deltaMs);
            }
        }
    }
}