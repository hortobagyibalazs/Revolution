using Revolution.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.ECS.Components
{
    public class StateMachineComponent : Component
    {
        public StateMachine StateMachine { get; set; }

        public StateMachineComponent()
        {

        }

        public StateMachineComponent(IState startState)
        {
            StateMachine = new StateMachine()
            {
                CurrentState = startState
            };
        }
    }
}
