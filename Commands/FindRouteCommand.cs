using Revolution.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.Commands
{
    public class FindRouteCommand
    {
        public Entity Entity { get; set; }
        public Vector2 Destination { get; set; }

        public FindRouteCommand(Entity entity, Vector2 destination)
        {
            Entity = entity;
            Destination = destination;
        }
    }
}
