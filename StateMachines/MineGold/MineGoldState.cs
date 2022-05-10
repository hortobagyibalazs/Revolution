using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.StateMachines.MineGold
{
    public class MineGoldState : IState
    {
        private Entity _entity;
        private static Random _random = new Random();

        public MineGoldState(Entity entity)
        {
            _entity = entity;
        }

        public void Exit()
        {

        }

        IState? IState.Execute()
        {
            var resourceComp = _entity.GetComponent<ResourceComponent>();

            if (resourceComp.Gold >= resourceComp.MaxGold)
            {
                return new DropGoldState(_entity);
            }
            else
            {
                if (_random.Next(1, GlobalConfig.PeasantGoldMiningRate) == 1)
                    resourceComp.Gold++;
            }

            return null;
        }
    }
}
