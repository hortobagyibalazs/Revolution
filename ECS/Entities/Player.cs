using Revolution.ECS.Components;
using Revolution.IO;
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
        public bool IsGuiControlled
        {
            get => GetComponent<ControlComponent>().IsGuiControlled; 
            set => GetComponent<ControlComponent>().IsGuiControlled = value;
        }

        public Player()
        {
            var teamComp = new TeamComponent();
            var resourceComp = new ResourceComponent() { Gold = GlobalConfig.StarterGold, Wood = GlobalConfig.StarterWood};
            var controlComp = new ControlComponent();

            AddComponent(teamComp);
            AddComponent(resourceComp);
            AddComponent(controlComp);
        }
    }
}
