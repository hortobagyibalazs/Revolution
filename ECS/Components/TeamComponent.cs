using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Revolution.ECS.Components
{
    public class TeamComponent
    {
        public int TeamId { get; set; }
        public Brushes TeamColor { get; set; }
    }
}
