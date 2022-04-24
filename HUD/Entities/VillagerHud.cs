using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.HUD.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Revolution.HUD.Entities
{
    public class VillagerHud : IEntityHud<Villager>
    {
        private IMessenger _messenger = Ioc.Default.GetService<IMessenger>();

        public HudComponent CreateComponent(Villager entity)
        {
            var hudComponent = new HudComponent();
            var panel = new UniformGrid();
            panel.Columns = 4;
            panel.Children.Add(
                new HudMiniActionButton(
                    new Uri(@"\Assets\Images\spr_farm_button.png", UriKind.Relative),
                    () => PurchaseBuilding<House>()
                )
            );
            panel.Children.Add(
                new HudMiniActionButton(
                    new Uri(@"\Assets\Images\spr_barracks_button.png", UriKind.Relative),
                    () => { }
                )
            );
            panel.Children.Add(
                new HudMiniActionButton(
                    new Uri(@"\Assets\Images\spr_stable_button.png", UriKind.Relative),
                    () => { }
                )
            );
            panel.Children.Add(
                new HudMiniActionButton(
                    new Uri(@"\Assets\Images\spr_tower_button.png", UriKind.Relative),
                    () => { }
                )
            );
            panel.Children.Add(
                new HudMiniActionButton(
                    new Uri(@"\Assets\Images\spr_town_hall_button.png", UriKind.Relative),
                    () => PurchaseBuilding<TownCenter>()
                )
            );
            hudComponent.Action = panel;
            hudComponent.Info = new Label();
            return hudComponent;
        }

        private void PurchaseBuilding<T>() where T : Entity
        {
            _messenger.Send(new BuildingPurchaseEvent(typeof(T)));
        }
    }
}
