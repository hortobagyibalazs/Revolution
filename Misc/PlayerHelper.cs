using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.Misc
{
    public class PlayerHelper
    {
        public static Player? GetGuiControlledPlayer()
        {
            foreach (var entity in EntityManager.GetEntities())
            {
                if (entity is Player)
                {
                    var controlComp = entity.GetComponent<ControlComponent>();
                    if (controlComp.IsGuiControlled)
                    {
                        return entity as Player;
                    }
                }
            }

            return null;
        }
    }
}
