using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;
using Revolution.ECS.Components;
using Revolution.HUD.Entities;
using Revolution.IO;

namespace Revolution.ECS.Entities
{
    internal class VillagerSpriteFrame
    {
        public static readonly SpriteFrame Idle = new SpriteFrame() { Source = new Uri(@"\Assets\Images\spr_peasant_standing.png", UriKind.Relative) };
        public static readonly SpriteFrame Moving = SpriteFrameSet.GetFirstFrame(@"\Assets\Images\spr_peasant_running");
        public static readonly SpriteFrame CutWood = SpriteFrameSet.GetFirstFrame(@"\Assets\Images\spr_peasant_attacking_lumber");
    }

    public class Villager : Entity
    {
        public Villager()
        {
            var sizeComp = new SizeComponent();
            var posComp = new PositionComponent();
            var spriteComp = new AnimatedSpriteComponent() { CurrentFrame = VillagerSpriteFrame.Idle };
            var gameMapObjectComp = new GameMapObjectComponent();
            var collisionComp = new CollisionComponent(gameMapObjectComp);
            var movementComp = new MovementComponent() { MaxVelocity = 4 };
            var selectionComp = new SelectionComponent(posComp, sizeComp);
            var directionComp = new DirectionComponent();
            var hudComp = new VillagerHud().CreateComponent(this);

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

            movementComp.PropertyChanged += delegate
            {
                if (movementComp.VelocityX != 0 || movementComp.VelocityY != 0)
                {
                    spriteComp.CurrentFrame = VillagerSpriteFrame.Moving;
                }
                else
                {
                    spriteComp.CurrentFrame = VillagerSpriteFrame.Idle;
                }
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
            AddComponent(directionComp);
            AddComponent(hudComp);
        }
    }
}