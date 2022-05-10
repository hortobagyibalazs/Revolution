using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using Revolution.Commands;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.StateMachines.CollectWood
{
    public abstract class DropResourcesState : IMoveState
    {
        protected Entity _entity;
        protected Vector2? _woodPos;

        private IMessenger _messenger = Ioc.Default.GetService<IMessenger>();

        public DropResourcesState(Entity entity)
        {
            var mapObjectComp = entity.GetComponent<GameMapObjectComponent>();

            _entity = entity;

            _woodPos = new Vector2(mapObjectComp.X, mapObjectComp.Y);

            _messenger.Send(new FindRouteToEntityTypeCommand(_entity, typeof(TownCenter)));
        }

        IState? IState.Execute()
        {
            var movementComp = _entity.GetComponent<MovementComponent>();
            if (movementComp != null && movementComp.CurrentTarget == null)
            {
                _messenger.Send(new DropResourcesCommand(_entity));
                return GetNextState();
            }

            return null;
        }

        public void Exit()
        {

        }

        protected abstract IState GetNextState();
    }
}
