using Revolution.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.Commands
{
    public class UnitPurchaseCommand
    {
        public Type UnitType { get; set; }
        public Entity Building { get; set; }

        public UnitPurchaseCommand(Type type, Entity building)
        {
            UnitType = type;
            Building = building;
        }
    }
}
