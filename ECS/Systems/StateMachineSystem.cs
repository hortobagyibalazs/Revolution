using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.ECS.Systems
{
    public class StateMachineSystem : ISystem
    {
        public void Update(int deltaMs)
        {
            foreach(var entity in EntityManager.GetEntities())
            {
                var stateMachineComp = entity.GetComponent<StateMachineComponent>();
                if (stateMachineComp != null)
                {
                    stateMachineComp.StateMachine.Execute();
                }
            }
        }
    }
}
