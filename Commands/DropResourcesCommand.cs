using Revolution.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.Commands
{
    public class DropResourcesCommand
    {
        public Entity Entity { get; set; }
        public DropResourcesCommand(Entity entity)
        {
            Entity = entity;
        }
    }
}
