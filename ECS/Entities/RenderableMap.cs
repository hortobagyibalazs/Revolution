using Revolution.ECS.Components;
using Revolution.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;

namespace Revolution.ECS.Entities
{
    public class RenderableMap : Entity
    {
        public List<Tile> Tiles { get; }

        public int Width 
        {
            get => (int) GetComponent<SizeComponent>().Width;
            set => GetComponent<SizeComponent>().Width = value;
        }
        public int Height
        {
            get => (int)GetComponent<SizeComponent>().Height;
            set => GetComponent<SizeComponent>().Height = value;
        }

        public RenderableMap()
        {
            Tiles = new List<Tile>();

            var renderComp = new RenderComponent() { 
                Renderable = new Image(), 
                ZIndex = -1
            };
            var posComp = new PositionComponent();
            var sizeComp = new SizeComponent();
            var minimapComp = new MinimapComponent();
            minimapComp.Draw += DrawToMinimap;

            AddComponent(renderComp);
            AddComponent(posComp);
            AddComponent(sizeComp);
            AddComponent(minimapComp);
        }

        public void InvalidateTiles()
        {
            var renderComp = GetComponent<RenderComponent>();
            var img = (renderComp.Renderable as System.Windows.Controls.Image);

            img.Source = DrawTileMap();
        }

        private ImageSource DrawTileMap()
        {
            DrawingVisual dv = new DrawingVisual();
            DrawingContext dc = dv.RenderOpen();
            foreach (var tile in Tiles)
            {
                var rect = new Rect(tile.CellX * GlobalConfig.TileSize, tile.CellY * GlobalConfig.TileSize, GlobalConfig.TileSize, GlobalConfig.TileSize);
                dc.DrawImage(tile.Drawable, rect);
            }
            dc.Close();

            RenderTargetBitmap rtb = new RenderTargetBitmap(Width, Height, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(dv);
            return rtb;
        }
        private void DrawToMinimap(object? sender, DrawingContext e)
        {

        }
    }
}
