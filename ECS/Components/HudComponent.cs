using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Revolution.ECS.Components
{
    public class HudComponent : Component
    {
        public Image Portrait { get; set; }
        public FrameworkElement Info { get; set; } // Center panel
        public FrameworkElement Action { get; set; } // Right panel
    }
}
