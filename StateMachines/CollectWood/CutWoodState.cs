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
        private int _woodCollected = 0;
        private int _woodCapacity = GlobalConfig.PeasantWoodCapacity;

        public CutWoodState(Entity entity)
        {
            _entity = entity;
        }

        public void Exit()
        {

        }

        IState? IState.Execute()
        {
            _woodCollected++;
            if (_woodCollected >= _woodCapacity)
            {
                return new DropResourcesState(_entity);
            }

            return null;
        }
    }
}
