using Revolution.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.StateMachines
{
    public interface IState
    {
        public StateMachine StateMachine { get; set; }
        void Execute();
    }
}
