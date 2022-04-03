using System.ComponentModel;
using System.Windows.Controls;
using Revolution.ECS.Components;
using Revolution.IO;

namespace Revolution.ECS.Entities
{
    public class Tile : Entity
    {
        public Tile()
        {
            var renderComp = new RenderComponent() {Renderable = new Image()};
            var sizeComp = new SizeComponent();
            var posComp = new PositionComponent();
            var mapObjectComp = new GameMapObjectComponent();
            var minimapComp = new MinimapComponent();
            
            sizeComp.PropertyChanged += delegate
            {
                (renderComp.Renderable).Width = sizeComp.Width;
                (renderComp.Renderable).Height = sizeComp.Height;
            };

            posComp.PropertyChanged += delegate
            {
                Canvas.SetLeft(renderComp.Renderable, posComp.X);
                Canvas.SetTop(renderComp.Renderable, posComp.Y);
            };
            
            mapObjectComp.PropertyChanged += delegate
            {
                sizeComp.Width = mapObjectComp.Width * GlobalConfig.TileSize;
                sizeComp.Height = mapObjectComp.Height * GlobalConfig.TileSize;
                posComp.X = mapObjectComp.X * GlobalConfig.TileSize;
                posComp.Y = mapObjectComp.Y * GlobalConfig.TileSize;
            };

            mapObjectComp.Width = 1;
            mapObjectComp.Height = 1;

            AddComponent(posComp);
            AddComponent(sizeComp);
            AddComponent(renderComp);
            AddComponent(mapObjectComp);
            AddComponent(minimapComp);
        }
    }
}