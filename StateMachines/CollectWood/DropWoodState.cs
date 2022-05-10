using Revolution.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.StateMachines.CollectWood
{
    public class DropWoodState : DropResourcesState
    {
        public DropWoodState(Entity entity) : base(entity)
        {
        }

        protected override IState GetNextState()
        {
            return new MoveToWoodState(_entity, _woodPos);
        }
    }
}
