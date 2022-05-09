using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.StateMachines
{
    public class StateMachine
    {
        private IState _current;
        public IState CurrentState 
        {
            get => _current;
            set
            {
                if (_current != null)
                {
                    _current.Exit();
                }
                _current = value;
                StateChanged?.Invoke(this, value);
            }
        }

        public EventHandler<IState> StateChanged;

        public void Execute()
        {
            var state = CurrentState?.Execute();
            if (state != null)
            {
                CurrentState = state;
            }
        }
    }
}
