using System.ComponentModel;
using System.Windows.Controls;
using Revolution.ECS.Components;
using Revolution.IO;

namespace Revolution.ECS.Entities
{
    public class TownCenter : Entity
    {
        public TownCenter()
        {
            var renderComp = new SpriteComponent() {Source = "Assets/town_center.png"};
            var posComp = new PositionComponent();
            var sizeComp = new SizeComponent();
            var mapObjectComp = new GameMapObjectComponent();
            var buildingComponent = new BuildingComponent() { State = BuildingState.Placing};
            var collisionComp = new CollisionComponent(mapObjectComp);
            
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
            mapObjectComp.Width = 3;
            mapObjectComp.Height = 3;
            
            AddComponent(renderComp);
            AddComponent(posComp);
            AddComponent(sizeComp);
            AddComponent(mapObjectComp);
            AddComponent(buildingComponent);
            AddComponent(collisionComp);
        }
    }
}