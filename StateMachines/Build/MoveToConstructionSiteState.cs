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

namespace Revolution.StateMachines.Build
{
    public class MoveToConstructionSiteState : IMoveState
    {
        private Villager _entity;
        private Entity _building;
        private IMessenger _messenger = Ioc.Default.GetService<IMessenger>();

        public MoveToConstructionSiteState(Villager entity, Entity building)
        {
            _entity = entity;
            _building = building;

            var mapObjectComp = building.GetComponent<GameMapObjectComponent>();
            Vector2 buildingPos = new Vector2(mapObjectComp.X, mapObjectComp.Y);

            _messenger.Send(new FindRouteCommand(_entity, buildingPos));
        }

        IState? IState.Execute()
        {
            var movementComp = _entity.GetComponent<MovementComponent>();
            if (movementComp != null && movementComp.CurrentTarget == null)
            {
                return new PeasantBuildingState(_entity, _building);
            }

            return null;
        }

        public void Exit()
        {

        }
    }
}
