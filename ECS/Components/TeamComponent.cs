using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Revolution.ECS.Components
{
    public class TeamComponent : Component
    {
        public int TeamId { get; set; }
        public SolidColorBrush TeamColor { get; set; }

        public void SetValuesFrom(TeamComponent src)
        {
            TeamId = src.TeamId;
            TeamColor = src.TeamColor;
        }
    }
}
