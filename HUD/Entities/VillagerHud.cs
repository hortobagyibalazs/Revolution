using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.HUD.Events;
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

        private FrameworkElement GetTooltipForBuilding(string name, int gold, int wood)
        {
            var buildingNameLabel = new Label();
            buildingNameLabel.Foreground = Brushes.White;
            buildingNameLabel.Content = name;

            var goldIcon = new Image() { Source = new BitmapImage(new Uri(@"\Assets\Images\spr_gold_coin_gui.png", UriKind.Relative)), Width=20, Height=20};
            var woodIcon = new Image() { Source = new BitmapImage(new Uri(@"\Assets\Images\spr_tree_gui.png", UriKind.Relative)), Width=20, Height=20};

            var goldLabel = new Label() { Content = gold, Foreground = Brushes.White };
            var woodLabel = new Label() { Content = wood, Foreground = Brushes.White };

            var resourceListView = new WrapPanel();
            resourceListView.Children.Add(woodIcon);
            resourceListView.Children.Add(woodLabel);
            resourceListView.Children.Add(goldIcon);
            resourceListView.Children.Add(goldLabel);

            var contentHolderView = new DockPanel();
            contentHolderView.Margin = new Thickness(16);
            contentHolderView.Children.Add(buildingNameLabel);
            contentHolderView.Children.Add(resourceListView);
            DockPanel.SetDock(buildingNameLabel, Dock.Top);
            DockPanel.SetDock(resourceListView, Dock.Bottom);

            return contentHolderView;
        }

        private FrameworkElement CreateBuyHouseButton()
        {
            int wood = 0;
            int gold = 0;
            GetPriceForBuilding<House>(ref wood, ref gold);
            var tooltip = GetTooltipForBuilding("House", gold, wood);

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
            int wood, gold;
            var tooltip = GetTooltipForBuilding("Barracks", 0, 0);

            var button = new HudMiniActionButton(
                    new Uri(@"\Assets\Images\spr_barracks_button.png", UriKind.Relative),
                    () => _messenger.Send(new ShowToastEvent("Not enough gold"))
                );

            return button;
        }

        private FrameworkElement CreateBuyStableButton()
        {
            var tooltip = GetTooltipForBuilding("Stable", 0, 0);

            var button = new HudMiniActionButton(
                    new Uri(@"\Assets\Images\spr_stable_button.png", UriKind.Relative),
                    () => _messenger.Send(new ShowToastEvent("Hello World!"))
                );

            return button;
        }

        private FrameworkElement CreateBuyTowerButton()
        {
            var tooltip = GetTooltipForBuilding("Tower", 0, 0);

            var button = new HudMiniActionButton(
                    new Uri(@"\Assets\Images\spr_tower_button.png", UriKind.Relative),
                    () => { }
                );

            return button;
        }

        private FrameworkElement CreateBuyTownCenterButton()
        {
            int wood = 0;
            int gold = 0;
            GetPriceForBuilding<TownCenter>(ref wood, ref gold);
            var tooltip = GetTooltipForBuilding("Town Center", 0, 0);

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
