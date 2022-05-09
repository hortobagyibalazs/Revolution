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
    public class MoveToWoodState : IMoveState
    {
        private Entity _entity;
        private IMessenger _messenger = Ioc.Default.GetService<IMessenger>();

        public MoveToWoodState(Entity entity, Vector2? woodPos = null)
        {
            _entity = entity;

            if (woodPos == null)
            {
                _messenger.Send(new FindRouteToEntityTypeCommand(_entity, typeof(Tree)));
            }
            else
            {
                _messenger.Send(new FindRouteCommand(_entity, (Vector2) woodPos));
            }
        }

        IState? IState.Execute()
        {
            var movementComp = _entity.GetComponent<MovementComponent>();
            if (movementComp != null && movementComp.CurrentTarget == null)
            {
                return new CutWoodState(_entity);
            } 

            return null;
        }

        public void Exit()
        {

        }
    }
}
