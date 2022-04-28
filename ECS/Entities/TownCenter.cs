using System;
using System.ComponentModel;
using System.Windows.Controls;
using Revolution.ECS.Components;
using Revolution.HUD.Entities;
using Revolution.IO;

namespace Revolution.ECS.Entities
{
    internal class TownCenterSpriteFrame
    {
        public static readonly SpriteFrame UnderConstruction = new SpriteFrame() { Source = new Uri(@"\Assets\Images\spr_construction_site.png", UriKind.Relative) };
        public static readonly SpriteFrame Normal = new SpriteFrame() { Source = new Uri(@"\Assets\Images\spr_town_hall.png", UriKind.Relative) };
    }

    public class TownCenter : Entity
    {
        public TownCenter()
        {
            var renderComp = new AnimatedSpriteComponent()
            {
                CurrentFrame = TownCenterSpriteFrame.Normal
            };
            var posComp = new PositionComponent();
            var sizeComp = new SizeComponent();
            var mapObjectComp = new GameMapObjectComponent();
            var buildingComponent = new BuildingComponent() { State = BuildingState.Placing};
            var collisionComp = new CollisionComponent(mapObjectComp);
            var selectionComp = new SelectionComponent(posComp, sizeComp);
            var spawnerComp = new SpawnerComponent();
            var hudComp = new TownCenterHud().CreateComponent(this);
            
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
                    renderComp.CurrentFrame = TownCenterSpriteFrame.UnderConstruction;
                }
                else
                {
                    renderComp.CurrentFrame = TownCenterSpriteFrame.Normal;
                }
            };

            mapObjectComp.X = 1;
            mapObjectComp.Y = 1;
            mapObjectComp.Width = 4;
            mapObjectComp.Height = 4;
            
            AddComponent(renderComp);
            AddComponent(posComp);
            AddComponent(sizeComp);
            AddComponent(mapObjectComp);
            AddComponent(buildingComponent);
            AddComponent(collisionComp);
            AddComponent(selectionComp);
            AddComponent(spawnerComp);
            AddComponent(hudComp);
        }
    }
}