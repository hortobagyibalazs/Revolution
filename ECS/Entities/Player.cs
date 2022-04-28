using Revolution.ECS.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.ECS.Entities
{
    // Can be either user or AI controlled
    public class Player : Entity
    {
        public Player()
        {
            var teamComp = new TeamComponent();
            var resourceComp = new ResourceComponent();

            AddComponent(teamComp);
            AddComponent(resourceComp);
        }
    }
}
