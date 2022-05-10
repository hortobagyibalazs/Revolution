using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.HUD.Controls;
using Revolution.HUD.Events;
using Revolution.IO;
using Revolution.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

            panel.Children.Add(CreateBuyHouseButton());
            panel.Children.Add(CreateBuyBarracksButton());
            panel.Children.Add(CreateBuyStableButton());
            panel.Children.Add(CreateBuyTowerButton());
            panel.Children.Add(CreateBuyTownCenterButton());

            hudComponent.Action = panel;
            hudComponent.Info = new Label();

            return hudComponent;
        }

        private FrameworkElement CreateBuyHouseButton()
        {
            int wood = GlobalConfig.HousePriceWood;
            int gold = GlobalConfig.HousePriceGold;
            var tooltip = TooltipHelper.GetBasicTooltipForEntityPurchase("House", gold, wood);

            var button = new HudMiniActionButton(
                    new Uri(@"\Assets\Images\spr_farm_button.png", UriKind.Relative),
                    () => PurchaseBuilding<House>(),
                    () => _messenger.Send(new ShowTooltipEvent(tooltip)),
                    () => _messenger.Send(new HideTooltipEvent(tooltip))
                );

            return button;
        }

        private FrameworkElement CreateBuyBarracksButton()
        {
            int wood = GlobalConfig.BarracksPriceWood;
            int gold = GlobalConfig.BarracksPriceGold;
            var tooltip = TooltipHelper.GetBasicTooltipForEntityPurchase("Barracks", gold, wood);

            var button = new HudMiniActionButton(
                    new Uri(@"\Assets\Images\spr_barracks_button.png", UriKind.Relative),
                    () => PurchaseBuilding<Barracks>(),
                    () => _messenger.Send(new ShowTooltipEvent(tooltip)),
                    () => _messenger.Send(new HideTooltipEvent(tooltip))
                );

            return button;
        }

        private FrameworkElement CreateBuyStableButton()
        {
            var tooltip = TooltipHelper.GetBasicTooltipForEntityPurchase("Stable", 0, 0);

            var button = new HudMiniActionButton(
                    new Uri(@"\Assets\Images\spr_stable_button.png", UriKind.Relative),
                    () => { }
                );

            return button;
        }

        private FrameworkElement CreateBuyTowerButton()
        {
            var tooltip = TooltipHelper.GetBasicTooltipForEntityPurchase("Tower", 0, 0);

            var button = new HudMiniActionButton(
                    new Uri(@"\Assets\Images\spr_tower_button.png", UriKind.Relative),
                    () => { }
                );

            return button;
        }

        private FrameworkElement CreateBuyTownCenterButton()
        {
            int wood = GlobalConfig.TownCenterPriceWood;
            int gold = GlobalConfig.TownCenterPriceGold;
            var tooltip = TooltipHelper.GetBasicTooltipForEntityPurchase("Town Center", gold, wood);

            var button = new HudMiniActionButton(
                    new Uri(@"\Assets\Images\spr_town_hall_button.png", UriKind.Relative),
                    () => PurchaseBuilding<TownCenter>(),
                    () => _messenger.Send(new ShowTooltipEvent(tooltip)),
                    () => _messenger.Send(new HideTooltipEvent(tooltip))
                );

            return button;
        }

        private void PurchaseBuilding<T>() where T : Entity
        {
            _messenger.Send(new BuildingPurchaseCommand(typeof(T), PlayerHelper.GetGuiControlledPlayer()));
        }

        private void GetPriceForBuilding<T>(ref int wood, ref int gold) where T : Entity
        {
            var building = EntityManager.CreateEntity<T>();
            var priceComp = building.GetComponent<PriceComponent>();
            if (priceComp != null)
            {
                wood = priceComp.Wood;
                gold = priceComp.Gold;
            }

            building.Destroy();
        }
    }
}
