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

        public bool Buy(ResourceComponent resource)
        {
            if (resource != null && resource.Wood >= Wood && resource.Gold >= Gold)
            {
                resource.Wood -= Wood;
                resource.Gold -= Gold;
                return true;
            }

            return false;
        }
    }
}
