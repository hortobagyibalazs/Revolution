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
                    _current.StateMachine = null;
                }
                _current = value;
                StateChanged?.Invoke(this, value);
                if (_current != null)
                {
                    _current.StateMachine = this;
                }
            }
        }

        public EventHandler<IState> StateChanged;

        public void Execute()
        {
            CurrentState.Execute();
        }
    }
}
