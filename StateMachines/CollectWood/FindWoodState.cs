using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using Revolution.Commands;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.IO;
using Revolution.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.StateMachines.CollectWood
{
    public class FindWoodState : IState
    {
        public StateMachine StateMachine { get; set; }

        private Entity _entity;
        private IMessenger _messenger = Ioc.Default.GetService<IMessenger>();

        public FindWoodState(Entity entity)
        {
            _entity = entity;
        }

        public void Execute()
        {
            var mapObjectComp = _entity.GetComponent<GameMapObjectComponent>();
            _messenger.Send(new FindRouteToEntityTypeCommand(_entity, typeof(Tree)));
            StateMachine.CurrentState = new WaitForArrivalState(_entity);
        }
    }
}
