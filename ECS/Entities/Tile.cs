using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Media;
using Revolution.ECS.Components;
using Revolution.IO;

namespace Revolution.ECS.Entities
{
    public class Tile : Entity
    {
        public Tile()
        {
            var renderComp = new RenderComponent() {Renderable = new Image()};
            var sizeComp = new SizeComponent() {Width = GlobalConfig.TileSize, Height = GlobalConfig.TileSize};
            var posComp = new PositionComponent();
            
            sizeComp.PropertyChanged += delegate(object? sender, PropertyChangedEventArgs args)
            {
                (renderComp.Renderable).Width = sizeComp.Width;
                (renderComp.Renderable).Height = sizeComp.Height;
            };

            posComp.PropertyChanged += delegate(object? sender, PropertyChangedEventArgs args)
            {
                Canvas.SetLeft(renderComp.Renderable, posComp.X);
                Canvas.SetTop(renderComp.Renderable, posComp.Y);
            };

            AddComponent(posComp);
            AddComponent(sizeComp);
            AddComponent(renderComp);
        }
    }
}