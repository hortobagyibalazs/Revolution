using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.StateMachines.CollectWood
{
    public class WaitForArrivalState : IState
    {
        public StateMachine StateMachine { get; set; }

        private Entity _entity;

        public WaitForArrivalState(Entity entity)
        {
            _entity = entity;
        }

        public void Execute()
        {
            var movementComp = _entity.GetComponent<MovementComponent>();
            if (movementComp != null && movementComp.CurrentTarget == null)
            {
                StateMachine.CurrentState = new CutWoodState();
            }
        }
    }
}
