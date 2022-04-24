﻿using Revolution.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.HUD.Events
{
    public class BuildingPurchaseEvent
    {
        public Type BuildingType { get; set; }

        public BuildingPurchaseEvent(Type type)
        {
            BuildingType = type;
        }
    }
}