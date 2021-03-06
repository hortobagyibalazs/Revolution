using Revolution.ECS.Components;
using Revolution.IO;
using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace Revolution.ECS.Entities
{
    internal class HouseSpriteFrame
    {
        public static readonly SpriteFrame UnderConstruction = SpriteFrameSet.GetFirstFrame(@"\Assets\Images\spr_farm_construction");

        public static readonly SpriteFrame Normal = new SpriteFrame() { Source = new Uri(@"\Assets\Images\spr_farm.png", UriKind.Relative) };
    }

    public class House : Entity
    {
        public House()
        {
            var renderComp = new AnimatedSpriteComponent()
            {
                CurrentFrame = HouseSpriteFrame.Normal,
                AutoAnimation = false
            };
            var posComp = new PositionComponent();
            var sizeComp = new SizeComponent();
            var mapObjectComp = new GameMapObjectComponent();
            var buildingComponent = new BuildingComponent() {State = BuildingState.Placing, BuildMaxProgress = GlobalConfig.HouseBuildPoints};
            var collisionComp = new CollisionComponent(mapObjectComp);
            var selectionComp = new SelectionComponent(posComp, sizeComp);
            var teamComp = new TeamComponent() { TeamColor = Brushes.DarkBlue };
            var resourceComp = new ResourceComponent() { MaxPopulation = 0 };
            var minimapComp = new MinimapComponent() { Background = teamComp.TeamColor };
            var priceComp = new PriceComponent() 
            {
                Wood = GlobalConfig.HousePriceWood,
                Gold = GlobalConfig.HousePriceGold 
            };
            
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

            buildingComponent.PropertyChanged += delegate
            {
                if (buildingComponent.State == BuildingState.UnderConstruction)
                {
                    renderComp.CurrentFrame = HouseSpriteFrame.UnderConstruction;
                }
                else
                {
                    renderComp.CurrentFrame = HouseSpriteFrame.Normal;
                    if (buildingComponent.State == BuildingState.Built)
                    {
                        resourceComp.MaxPopulation = 5;
                    }
                }
            };

            buildingComponent.BuildProgressChanged += delegate
            {
                if (buildingComponent.BuildProgress > 0 
                    && buildingComponent.BuildProgress % 167 == 0 // 500 / 3 == 167 (3 different sprites, 500 build points)
                    && buildingComponent.BuildProgress < buildingComponent.BuildMaxProgress)
                {
                    renderComp.NextFrame();
                }
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
            AddComponent(priceComp);
            AddComponent(resourceComp);
        }
    }
}