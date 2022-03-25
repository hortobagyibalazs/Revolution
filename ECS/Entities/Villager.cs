using System.ComponentModel;
using Avalonia.Controls;
using Revolution.ECS.Components;
using Revolution.IO;

namespace Revolution.ECS.Entities
{
    public class Villager : Entity
    {
        public Villager()
        {
            var sizeComp = new SizeComponent();
            var posComp = new PositionComponent();
            var spriteComp = new SpriteComponent() {Source = "Assets/villager.png"};
            var gameMapObjectComp = new GameMapObjectComponent();
            var collisionComp = new CollisionComponent(sizeComp, posComp);
            var movementComp = new MovementComponent() { MaxVelocity = 2, VelocityX = 2, TargetTileDeltaX = 1};
            
            gameMapObjectComp.PropertyChanged += delegate
            {
                sizeComp.Width = gameMapObjectComp.Width * GlobalConfig.TileSize;
                sizeComp.Height = gameMapObjectComp.Height * GlobalConfig.TileSize;
            };
            
            sizeComp.PropertyChanged += delegate
            {
                (spriteComp.Renderable).Width = sizeComp.Width;
                (spriteComp.Renderable).Height = sizeComp.Height;
            };

            posComp.PropertyChanged += delegate
            {
                gameMapObjectComp.X = posComp.X / GlobalConfig.TileSize;
                gameMapObjectComp.Y = posComp.Y / GlobalConfig.TileSize;
                
                Canvas.SetLeft(spriteComp.Renderable, posComp.X);
                Canvas.SetTop(spriteComp.Renderable, posComp.Y);
            };

            gameMapObjectComp.Width = 1;
            gameMapObjectComp.Height = 1;
            gameMapObjectComp.X = 0;
            gameMapObjectComp.Y = 1;
            
            AddComponent(sizeComp);
            AddComponent(posComp);
            AddComponent(spriteComp);
            AddComponent(gameMapObjectComp);
            AddComponent(collisionComp);
            AddComponent(movementComp);
        }
    }
}