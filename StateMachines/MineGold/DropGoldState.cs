using Revolution.ECS.Entities;
using Revolution.StateMachines.CollectWood;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.StateMachines.MineGold
{
    public class DropGoldState : DropResourcesState
    {
        public DropGoldState(Entity entity) : base(entity)
        {

        }

        protected override IState GetNextState()
        {
            return new MoveToMineState(_entity);
        }
    }
}
