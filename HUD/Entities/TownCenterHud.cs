using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using Revolution.Commands;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.HUD.Controls;
using Revolution.HUD.Events;
using Revolution.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Revolution.HUD.Entities
{
    public class TownCenterHud : IEntityHud<TownCenter>
    {
        private IMessenger _messenger = Ioc.Default.GetService<IMessenger>();
        private TownCenter _entity;

        public HudComponent CreateComponent(TownCenter entity)
        {
            _entity = entity;

            var comp = new HudComponent();
            var panel = new UniformGrid();
            panel.Columns = 4;
            panel.Children.Add(CreateBuyPeasantButton());

            comp.Info = new Label();
            comp.Action = panel;
            return comp;
        }

        private FrameworkElement CreateBuyPeasantButton()
        {
            int wood = GlobalConfig.PeasantPriceWood;
            int gold = GlobalConfig.PeasantPriceGold;
            var tooltip = TooltipHelper.GetBasicTooltipForEntityPurchase("Villager", gold, wood);

            var button = new HudMiniActionButton(
                    new Uri(@"\Assets\Images\spr_peasant_button.png", UriKind.Relative),
                    () => PurchaseUnit<Villager>(),
                    () => _messenger.Send(new ShowTooltipEvent(tooltip)),
                    () => _messenger.Send(new HideTooltipEvent(tooltip))
                );

            return button;
        }

        private void PurchaseUnit<T>() where T : Entity
        {
            _messenger.Send(new UnitPurchaseCommand(typeof(T), _entity));
        }
    }
}
