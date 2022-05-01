using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Revolution.HUD.Entities
{
    public class TownCenterHud : IEntityHud<TownCenter>
    {
        public HudComponent CreateComponent(TownCenter entity)
        {
            var comp = new HudComponent();
            var panel = new UniformGrid();
            panel.Columns = 4;
            panel.Children.Add(
                new HudMiniActionButton(
                    new Uri(@"\Assets\Images\spr_peasant_button.png", UriKind.Relative),
                    () => entity.GetComponent<SpawnerComponent>().SpawnQueue.Enqueue(typeof(Villager))
                )
            );

            comp.Info = new Label();
            comp.Action = panel;
            return comp;
        }
    }
}
