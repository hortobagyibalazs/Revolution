using Revolution.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.Commands
{
    public class FindRouteToEntityTypeCommand
    {
        public Entity Entity { get; set; }
        public Type DestEntityType { get; set; }
        public Vector2? ApproximatePosition { get; set; }

        public FindRouteToEntityTypeCommand(Entity entity, Type entityType, Vector2? approxPos = null)
        {
            Entity = entity;
            DestEntityType = entityType;
            ApproximatePosition = approxPos;
        }
    }
}
