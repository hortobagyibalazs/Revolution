using Revolution.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.ECS.Components
{
    public class SpawnerComponent : Component
    {
        public Queue<Type> SpawnQueue { get; set; }
        public int Capacity { get; set; }
        public Vector2 SpawnTarget { get; set; }

        public SpawnerComponent()
        {
            SpawnQueue = new Queue<Type>();
        }
    }
}
