using Revolution.ECS.Components;
using Revolution.HUD.Entities;
using Revolution.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Revolution.ECS.Entities
{
    public class Barracks : Entity
    {
        internal class BarracksSpriteFrame
        {
            public static readonly SpriteFrame UnderConstruction = SpriteFrameSet.GetFirstFrame(@"\Assets\Images\spr_barracks_construction");
            public static readonly SpriteFrame Normal = new SpriteFrame() { Source = new Uri(@"\Assets\Images\spr_barracks.png", UriKind.Relative) };
        }

        public Barracks()
        {
                var renderComp = new AnimatedSpriteComponent()
                {
                    CurrentFrame = BarracksSpriteFrame.Normal,
                    AutoAnimation = false
                };
                var posComp = new PositionComponent();
                var sizeComp = new SizeComponent();
                var mapObjectComp = new GameMapObjectComponent();
                var buildingComponent = new BuildingComponent() { State = BuildingState.Placing, BuildMaxProgress = GlobalConfig.BarracksBuildPoints };
                var collisionComp = new CollisionComponent(mapObjectComp);
                var selectionComp = new SelectionComponent(posComp, sizeComp);
                var spawnerComp = new SpawnerComponent();
                var teamComp = new TeamComponent();
                var priceComp = new PriceComponent()
                {
                    Wood = GlobalConfig.BarracksPriceWood,
                    Gold = GlobalConfig.BarracksPriceGold
                };
                var hudComp = new BarracksHud().CreateComponent(this);

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

                    spawnerComp.SpawnTarget = new System.Numerics.Vector2(mapObjectComp.X + 1, mapObjectComp.Y + mapObjectComp.Height);
                };

            buildingComponent.PropertyChanged += delegate
            {
                if (buildingComponent.State == BuildingState.UnderConstruction)
                {
                    renderComp.CurrentFrame = BarracksSpriteFrame.UnderConstruction;
                }
                else
                {
                    renderComp.CurrentFrame = BarracksSpriteFrame.Normal;
                    if (buildingComponent.State == BuildingState.Built)
                        AddComponent(hudComp);
                }
            };

            buildingComponent.BuildProgressChanged += delegate
            {
                if (buildingComponent.BuildProgress > 0
                    && buildingComponent.BuildProgress % (buildingComponent.BuildMaxProgress/3+1) == 0 // 500 / 3 == 167 (3 different sprites, 500 build points)
                    && buildingComponent.BuildProgress < buildingComponent.BuildMaxProgress)
                {
                    renderComp.NextFrame();
                }
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
                AddComponent(selectionComp);
                AddComponent(spawnerComp);
                AddComponent(teamComp);
                AddComponent(priceComp);
            }
    }
}
