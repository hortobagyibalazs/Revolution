using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Revolution.ECS.Components
{
    public class PlayerInputComponent : Component
    {
        public MouseEventHandler MouseMoveEventHandler;
        public MouseButtonEventHandler MouseButtonUpEventHandler;
        public MouseButtonEventHandler MouseButtonDownEventHandler;
    }
}
