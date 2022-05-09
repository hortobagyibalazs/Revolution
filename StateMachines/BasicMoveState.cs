using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using Revolution.Commands;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.StateMachines.Idle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.StateMachines
{
    public class BasicMoveState : IMoveState
    {
        private Entity _entity;
        private IMessenger _messenger = Ioc.Default.GetService<IMessenger>();

        public BasicMoveState(Entity entity, Vector2 pos)
        {
            _entity = entity;
            _messenger.Send(new FindRouteCommand(_entity, pos));
        }

        IState? IState.Execute()
        {
            var movementComp = _entity.GetComponent<MovementComponent>();
            if (movementComp != null && movementComp.CurrentTarget == null)
            {
                return new IdleState();
            }

            return null;
        }

        public void Exit()
        {

        }
    }
}
