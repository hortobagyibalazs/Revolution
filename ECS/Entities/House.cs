using Revolution.ECS.Components;
using Revolution.IO;
using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace Revolution.ECS.Entities
{
    public class House : Entity
    {
        public House()
        {
            var renderComp = new SpriteComponent() {Source = new Uri(@"\Assets\Images\spr_farm.png", UriKind.Relative)};
            var posComp = new PositionComponent();
            var sizeComp = new SizeComponent();
            var mapObjectComp = new GameMapObjectComponent();
            var buildingComponent = new BuildingComponent() {State = BuildingState.Placing};
            var collisionComp = new CollisionComponent(mapObjectComp);
            var selectionComp = new SelectionComponent(posComp, sizeComp);
            var teamComp = new TeamComponent() { TeamColor = Brushes.DarkBlue };
            var minimapComp = new MinimapComponent() { Background = teamComp.TeamColor };
            
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
            AddComponent(selectionComp);
            AddComponent(teamComp);
            AddComponent(minimapComp);
        }
    }
}