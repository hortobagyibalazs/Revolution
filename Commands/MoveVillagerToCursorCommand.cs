using Revolution.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.Commands
{
    public class MoveVillagerToCursorCommand
    {
        public Entity Entity { get; set; }
        public MoveVillagerToCursorCommand(Entity entity)
        {
            Entity = entity;
        }
    }
}
