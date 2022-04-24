using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.HUD.Entities
{
    public interface IEntityHud<T> where T : Entity
    {
        HudComponent CreateComponent(T entity);
    }
}
