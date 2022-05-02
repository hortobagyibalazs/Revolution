using Revolution.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.HUD.Events
{
    public class BuildingPurchaseCommand
    {
        public Type BuildingType { get; set; }
        public Player Player { get; set; }

        public BuildingPurchaseCommand(Type type, Player sender)
        {
            BuildingType = type;
            Player = sender;
        }
    }
}
