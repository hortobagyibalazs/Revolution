using Revolution.ECS.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Revolution.HUD.Events
{
    public class HideTooltipEvent
    {
        public readonly FrameworkElement Drawable;

        public HideTooltipEvent(FrameworkElement drawable)
        {
            Drawable = drawable;
        }
    }
}
