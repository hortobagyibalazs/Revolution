using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.Commands
{
    public class BuildWithPeasantCommand
    {
        public Villager Peasant { get; set; }
        public Entity Building { get; set; }

        public BuildWithPeasantCommand(Villager villager, Entity building)
        {
            Peasant = villager;
            Building = building;
        }

        public BuildWithPeasantCommand(Entity building)
        {
            Peasant = FindSelectedPeasant();
            Building = building;
        }

        private Villager FindSelectedPeasant()
        {
            foreach(var entity in EntityManager.GetEntities())
            {
                if (entity is Villager && entity.GetComponent<SelectionComponent>().Selected)
                {
                    return (Villager) entity;
                }
            }

            return null;
        }
    }
}
