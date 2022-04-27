using Revolution.ECS.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Revolution.HUD.Events
{
    public class ShowTooltipEvent
    {
        public readonly FrameworkElement Drawable;

        public ShowTooltipEvent(FrameworkElement drawable)
        {
            Drawable = drawable;
        }
    }
}
