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
    public class BarracksHud : IEntityHud<Barracks>
    {
        private IMessenger _messenger = Ioc.Default.GetService<IMessenger>();
        private Barracks _entity;

        public HudComponent CreateComponent(Barracks entity)
        {
             _entity = entity;

            var comp = new HudComponent();
            var panel = new UniformGrid();
            panel.Columns = 4;
            panel.Children.Add(CreateBuySwordsmanButton());
            panel.Children.Add(CreateBuyArcherButton());

            comp.Info = new Label();
            comp.Action = panel;
            return comp;
        }

        private FrameworkElement CreateBuySwordsmanButton()
        {
            int wood = GlobalConfig.SwordsmanPriceWood;
            int gold = GlobalConfig.SwordsmanPriceGold;
            var tooltip = TooltipHelper.GetBasicTooltipForEntityPurchase("Swordsman", gold, wood);

            var button = new HudMiniActionButton(
                new Uri(@"\Assets\Images\spr_footman_button.png", UriKind.Relative),
                () => PurchaseUnit<Swordsman>(),
                () => _messenger.Send(new ShowTooltipEvent(tooltip)),
                () => _messenger.Send(new HideTooltipEvent(tooltip))
            );

            return button;
        }

        private FrameworkElement CreateBuyArcherButton()
        {
            int wood = GlobalConfig.ArcherPriceWood;
            int gold = GlobalConfig.ArcherPriceGold;
            var tooltip = TooltipHelper.GetBasicTooltipForEntityPurchase("Archer", gold, wood);

            var button = new HudMiniActionButton(
                new Uri(@"\Assets\Images\spr_archer_button.png", UriKind.Relative),
                () => PurchaseUnit<Archer>(),
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
