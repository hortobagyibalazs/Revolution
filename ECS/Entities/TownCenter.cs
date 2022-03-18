using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Revolution.ECS.Components;
using Revolution.IO;

namespace Revolution.ECS.Entities
{
    public class TownCenter : Entity
    {
        public TownCenter()
        {
            var renderComp = new RenderComponent() {Renderable = new Image() {Source = new Bitmap("Assets/town_center.png")}}; 
            var posComp = new PositionComponent() {};
            var sizeComp = new SizeComponent() {Width = 3 * GlobalConfig.TileSize, Height = 3 * GlobalConfig.TileSize};
            var buildingComponent = new BuildingComponent() {State = BuildingState.Built};
            
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
            
            AddComponent(renderComp);
            AddComponent(posComp);
            AddComponent(sizeComp);
            AddComponent(buildingComponent);
        }
    }
}