using Revolution.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Revolution.ECS.Components
{
    public class MinimapComponent : Component
    {
        public Brush Background { get; set; }

        public EventHandler<MinimapDrawEventArgs> Draw;
    }

    public class MinimapDrawEventArgs : EventArgs
    {
        public DrawingContext DrawingContext { get; set; }
        public Canvas MinimapCanvas { get; set; }
        public MapData GameMap { get; set; }

        public MinimapDrawEventArgs(DrawingContext dc, Canvas canvas, MapData map)
        {
            DrawingContext = dc;
            MinimapCanvas = canvas;
            GameMap = map;
        }
    }
}
