using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Revolution.ECS.Components;
using Revolution.HUD.Entities;
using Revolution.HUD.EventHandlers;
using Revolution.IO;
using Revolution.StateMachines;
using Revolution.StateMachines.Build;
using Revolution.StateMachines.CollectWood;
using Revolution.StateMachines.Idle;
using Revolution.StateMachines.MineGold;

namespace Revolution.ECS.Entities
{
    internal class VillagerSpriteFrame
    {
        public static readonly SpriteFrame Idle = new SpriteFrame() { Source = new Uri(@"\Assets\Images\spr_peasant_standing.png", UriKind.Relative) };
        public static readonly SpriteFrame Moving = SpriteFrameSet.GetFirstFrame(@"\Assets\Images\spr_peasant_running");
        public static readonly SpriteFrame CutWood = SpriteFrameSet.GetFirstFrame(@"\Assets\Images\spr_peasant_attacking_lumber");
        public static readonly SpriteFrame CarryResources = SpriteFrameSet.GetFirstFrame(@"\Assets\Images\spr_peasant_carrying_gold");
        public static readonly SpriteFrame Build = SpriteFrameSet.GetFirstFrame(@"\Assets\Images\spr_peasant_attacking");
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
            var selectionComp = new SelectionComponent(posComp, sizeComp) { MultiSelectable = true };
            var directionComp = new DirectionComponent();
            var priceComp = new PriceComponent()
            {
                Gold = 50
            };
            var smComp = new StateMachineComponent(new IdleState());
            var resourceComp = new ResourceComponent()
            { 
                Population = 1,
                MaxWood = GlobalConfig.PeasantWoodCapacity, 
                MaxGold = GlobalConfig.PeasantGoldCapacity 
            };
            var teamComp = new TeamComponent();
            var hudComp = new VillagerHud().CreateComponent(this);
            var inputComp = new PlayerInputComponent();

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
                    spriteComp.CurrentFrame = VillagerSpriteFrame.Idle;
                }
                else if (state is DropResourcesState)
                {
                    spriteComp.CurrentFrame = VillagerSpriteFrame.CarryResources;
                }
                else if (state is PeasantBuildingState)
                {
                    spriteComp.CurrentFrame = VillagerSpriteFrame.Build;
                }
                else if (state is IMoveState)
                {
                    spriteComp.CurrentFrame = VillagerSpriteFrame.Moving;
                }
                else if (state is CutWoodState)
                {
                    spriteComp.CurrentFrame = VillagerSpriteFrame.CutWood;
                }
                else if (state is MineGoldState)
                {
                    spriteComp.CurrentFrame = VillagerSpriteFrame.Build;
                }
            };

            var vih = new VillagerInputHandler(this);
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
            AddComponent(hudComp);
            AddComponent(inputComp);
            AddComponent(teamComp);
            AddComponent(resourceComp);
            AddComponent(priceComp);
        }
    }
}