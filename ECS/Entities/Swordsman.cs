using Revolution.ECS.Components;
using Revolution.HUD.EventHandlers;
using Revolution.IO;
using Revolution.StateMachines;
using Revolution.StateMachines.Idle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Revolution.ECS.Entities
{
    internal class SwordsmanSpriteFrame
    {
        public static readonly SpriteFrame Idle = new SpriteFrame() { Source = new Uri(@"\Assets\Images\spr_footman_standing.png", UriKind.Relative) };
        public static readonly SpriteFrame Moving = SpriteFrameSet.GetFirstFrame(@"\Assets\Images\spr_footman_running");
    }

    public class Swordsman : Entity
    {
        public Swordsman()
        {
            var sizeComp = new SizeComponent();
            var posComp = new PositionComponent();
            var spriteComp = new AnimatedSpriteComponent() { CurrentFrame = SwordsmanSpriteFrame.Idle };
            var gameMapObjectComp = new GameMapObjectComponent();
            var collisionComp = new CollisionComponent(gameMapObjectComp);
            var movementComp = new MovementComponent() { MaxVelocity = 2 };
            var selectionComp = new SelectionComponent(posComp, sizeComp) { MultiSelectable = true };
            var directionComp = new DirectionComponent();
            var priceComp = new PriceComponent()
            {
                Gold = GlobalConfig.SwordsmanPriceGold,
                Wood = GlobalConfig.SwordsmanPriceWood
            };
            var smComp = new StateMachineComponent(new IdleState());
            var teamComp = new TeamComponent();
            var inputComp = new PlayerInputComponent();
            var resourceComp = new ResourceComponent() { Population = 1 };

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

            smComp.StateMachine.StateChanged += delegate
            {
                var state = smComp.StateMachine.CurrentState;
                if (state is IdleState)
                {
                    spriteComp.CurrentFrame = SwordsmanSpriteFrame.Idle;
                }
                else if (state is IMoveState)
                {
                    spriteComp.CurrentFrame = SwordsmanSpriteFrame.Moving;
                }
            };

            var vih = new TrooperInputHandler(this);
            inputComp.MouseButtonDownEventHandler += vih.RightButtonDownHandler;

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
            AddComponent(smComp);
            AddComponent(inputComp);
            AddComponent(teamComp);
            AddComponent(priceComp);
            AddComponent(resourceComp);
        }
    }
}
