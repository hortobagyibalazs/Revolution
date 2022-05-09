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
    public class CutWoodState : IState
    {
        private Entity _entity;
        private static Random _random = new Random();

        public CutWoodState(Entity entity)
        {
            _entity = entity;
        }

        public void Exit()
        {

        }

        IState? IState.Execute()
        {
            var resourceComp = _entity.GetComponent<ResourceComponent>();

            if (resourceComp.Wood >= resourceComp.MaxWood)
            {
                return new DropResourcesState(_entity);
            } 
            else
            {
                if (_random.Next(1, 11) == 1)
                    resourceComp.Wood++;
            }

            return null;
        }
    }
}
