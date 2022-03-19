using Avalonia.Controls;
using Revolution.ECS.Components;
using Revolution.IO;

namespace Revolution.ECS.Entities
{
    public class House : Entity
    {
        public House()
        {
            var renderComp = new SpriteComponent() {Source = "Assets/house.png"}; 
            var posComp = new PositionComponent() {X = GlobalConfig.TileSize * 3, Y = GlobalConfig.TileSize * 4};
            var sizeComp = new SizeComponent() {Width = 2 * GlobalConfig.TileSize, Height = 2 * GlobalConfig.TileSize};
            var mapObjectComp = new GameMapObjectComponent();
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
            
            mapObjectComp.PropertyChanged += delegate
            {
                sizeComp.Width = mapObjectComp.Width * GlobalConfig.TileSize;
                sizeComp.Height = mapObjectComp.Height * GlobalConfig.TileSize;
                posComp.X = mapObjectComp.X * GlobalConfig.TileSize;
                posComp.Y = mapObjectComp.Y * GlobalConfig.TileSize;
            };
            
            AddComponent(renderComp);
            AddComponent(posComp);
            AddComponent(sizeComp);
            AddComponent(mapObjectComp);
            AddComponent(buildingComponent);
        }
    }
}