using System.ComponentModel;
using System.Windows.Controls;
using Revolution.ECS.Components;
using Revolution.IO;

namespace Revolution.ECS.Entities
{
    public class Villager : Entity
    {
        public Villager()
        {
            var sizeComp = new SizeComponent();
            var posComp = new PositionComponent() { X = 512, Y = 576 };  
            var spriteComp = new SpriteComponent() {Source = "Assets/villager.png"};
            var gameMapObjectComp = new GameMapObjectComponent();
            var collisionComp = new CollisionComponent(sizeComp, posComp);
            var movementComp = new MovementComponent() { MaxVelocity = 2 };
            var selectionComp = new SelectionComponent();
            
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

            gameMapObjectComp.X = posComp.X / GlobalConfig.TileSize;
            gameMapObjectComp.Y = posComp.Y / GlobalConfig.TileSize;

            AddComponent(sizeComp);
            AddComponent(posComp);
            AddComponent(spriteComp);
            AddComponent(gameMapObjectComp);
            AddComponent(collisionComp);
            AddComponent(movementComp);
            AddComponent(selectionComp);
        }
    }
}