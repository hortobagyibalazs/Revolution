using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.ECS.Components
{
    public class ControlComponent : Component
    {
        public bool IsGuiControlled { get; set; }
    }
}
