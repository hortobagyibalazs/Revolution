using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.ECS.Components
{
    public class PriceComponent : Component
    {
        public int Wood { get; set; }
        public int Gold { get; set; }
    }
}
