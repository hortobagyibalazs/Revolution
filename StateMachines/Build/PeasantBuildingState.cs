using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.StateMachines.Idle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.StateMachines.Build
{
    public class PeasantBuildingState : IState
    {
        private Entity _building;

        public PeasantBuildingState(Villager peasant, Entity building)
        {
            _building = building;
        }

        public IState? Execute()
        {
            // if done, return new IdleState
            var buildingComp = _building.GetComponent<BuildingComponent>();
            if (buildingComp.State == BuildingState.UnderConstruction)
            {
                buildingComp.BuildProgress++;
                return null;
            }

            // else
            return new IdleState();
        }

        public void Exit()
        {
            
        }
    }
}
