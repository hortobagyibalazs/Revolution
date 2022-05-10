using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using Revolution.Commands;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.StateMachines.MineGold
{
    public class MoveToMineState : IMoveState
    {
        private Entity _entity;
        private IMessenger _messenger = Ioc.Default.GetService<IMessenger>();

        public MoveToMineState(Entity entity, Vector2? minePos = null)
        {
            _entity = entity;

            _messenger.Send(new FindRouteToEntityTypeCommand(_entity, typeof(Goldmine), minePos));
        }

        IState? IState.Execute()
        {
            var movementComp = _entity.GetComponent<MovementComponent>();
            if (movementComp != null && movementComp.CurrentTarget == null)
            {
                return new MineGoldState(_entity);
            }

            return null;
        }

        public void Exit()
        {

        }
    }
}
