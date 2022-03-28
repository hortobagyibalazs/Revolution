using Revolution.ECS.Components;
using Revolution.IO;
using System.Windows.Controls;

namespace Revolution.ECS.Entities
{
    public class House : Entity
    {
        public House()
        {
            var renderComp = new SpriteComponent() {Source = "Assets/house.png"};
            var posComp = new PositionComponent();
            var sizeComp = new SizeComponent();
            var mapObjectComp = new GameMapObjectComponent();
            var buildingComponent = new BuildingComponent() {State = BuildingState.Placing};
            var collisionComp = new CollisionComponent(sizeComp, posComp);
            
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
            
            mapObjectComp.X = 1;
            mapObjectComp.Y = 1;
            mapObjectComp.Width = 2;
            mapObjectComp.Height = 2;
            
            AddComponent(renderComp);
            AddComponent(posComp);
            AddComponent(sizeComp);
            AddComponent(mapObjectComp);
            AddComponent(buildingComponent);
            AddComponent(collisionComp);
        }
    }
}