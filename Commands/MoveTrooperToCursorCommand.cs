using Revolution.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.Commands
{
    public class MoveTrooperToCursorCommand
    {
        public Entity Entity { get; set; }
        public MoveTrooperToCursorCommand(Entity entity)
        {
            Entity = entity;
        }
    }
}
