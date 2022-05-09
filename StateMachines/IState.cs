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
        IState? Execute();
        void Exit();
    }
}
