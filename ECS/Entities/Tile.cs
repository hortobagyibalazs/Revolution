using System.ComponentModel;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Revolution.ECS.Components;
using Revolution.IO;

namespace Revolution.ECS.Entities
{
    public class Tile
    {
        public BitmapSource Drawable { get; set; }
        public int CellX { get; set; }
        public int CellY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}