﻿using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using Revolution.Commands;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.IO;
using Revolution.Misc;
using Revolution.StateMachines;
using Revolution.StateMachines.CollectWood;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Revolution.ECS.Systems
{
    public class UnitCommandProcessorSystem : ISystem, IRecipient<MoveVillagerToCursorCommand>
    {
        private ScrollViewer _scrollViewer;
        private MapData _map;

        private IMessenger _messenger = Ioc.Default.GetService<IMessenger>();

        public UnitCommandProcessorSystem(ScrollViewer scrollViewer, MapData map)
        {
            _scrollViewer = scrollViewer;
            _map = map;

            _messenger.Register<MoveVillagerToCursorCommand>(this);
        }

        public void Receive(MoveVillagerToCursorCommand message)
        {
            var stateMachine = message.Entity.GetComponent<StateMachineComponent>()?.StateMachine;
            if (stateMachine != null)
            {
                int cellX = MapHelper.GetGameObjectPosBasedOnCursorX(_scrollViewer);
                int cellY = MapHelper.GetGameObjectPosBasedOnCursorY(_scrollViewer);

                // if target cell is entry, move
                if (MapHelper.CellAvailable(_map, cellX, cellY))
                {
                    stateMachine.CurrentState = new BasicMoveState(message.Entity, new Vector2(
                         cellX,
                         cellY
                         ));
                }
                // else if target cell contains a tree, move and start cutting
                else if (_map.Tiles[cellX, cellY].Exists(tile => tile.EntityType == typeof(Tree)))
                {
                    stateMachine.CurrentState = new MoveToWoodState(message.Entity, new Vector2(cellX, cellY));
                }
                else
                {
                    ;
                }
            }
        }

        public void Update(int deltaMs)
        {
            
        }
    }
}
