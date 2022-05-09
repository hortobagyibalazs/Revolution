using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.ECS.Components
{
    public class ResourceComponent : Component
    {
        public int Wood { get; set; }
        public int Gold { get; set; }
        public int Population { get; set; }

        public int MaxWood { get; set; }
        public int MaxGold { get; set; }
        public int MaxPopulation { get; set; }
    }
}
