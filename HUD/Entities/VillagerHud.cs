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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

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
            var _buildingName = new Label();
            _buildingName.Foreground = Brushes.White;
            _buildingName.Content = name;
            var _tooltipView = new Grid();
            _tooltipView.Margin = new Thickness(16);
            _tooltipView.Children.Add(_buildingName);
            return _tooltipView;
        }

        private FrameworkElement CreateBuyHouseButton()
        {
            var tooltip = GetTooltipForBuilding("House", 0, 0);

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
            var tooltip = GetTooltipForBuilding("Barracks", 0, 0);

            var button = new HudMiniActionButton(
                    new Uri(@"\Assets\Images\spr_barracks_button.png", UriKind.Relative),
                    () => { }
                );

            return button;
        }

        private FrameworkElement CreateBuyStableButton()
        {
            var tooltip = GetTooltipForBuilding("Stable", 0, 0);

            var button = new HudMiniActionButton(
                    new Uri(@"\Assets\Images\spr_stable_button.png", UriKind.Relative),
                    () => { }
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
            _messenger.Send(new BuildingPurchaseEvent(typeof(T)));
        }
    }
}
