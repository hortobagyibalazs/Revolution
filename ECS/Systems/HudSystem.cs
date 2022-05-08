using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Revolution.ECS.Systems
{
    public class HudSystem : ISystem
    {
        private Panel _centerPanel;
        private Panel _rightPanel;

        private Label _playerWood;
        private Label _playerGold;
        private Label _playerPopulation;

        HashSet<Entity> _selectedEntities;

        public HudSystem(Panel center, Panel right, Label wood, Label gold, Label population)
        {
            _centerPanel = center;
            _rightPanel = right;

            _playerWood = wood;
            _playerGold = gold;
            _playerPopulation = population;

            _selectedEntities = new HashSet<Entity>();
        }
        
        public void Update(int deltaMs)
        {
            _selectedEntities.Clear();
            foreach(var entity in EntityManager.GetEntities())
            {
                if (entity is Player && (entity as Player).IsGuiControlled)
                {
                    UpdateLabels(entity as Player);
                    continue;
                }

                var selectionComp = entity.GetComponent<SelectionComponent>();
                var hudComp = entity.GetComponent<HudComponent>();
                if (selectionComp != null && selectionComp.Selected && hudComp != null)
                {
                    _selectedEntities.Add(entity);
                }
            }
            
            if (_selectedEntities.Count == 0)
            {
                DisplayNoGui();
            }
            else if (_selectedEntities.Count == 1)
            {
                var hudComp = _selectedEntities.First().GetComponent<HudComponent>();
                DisplaySingleSelectionGui(hudComp);
            }
            else
            {
                var hudComps = _selectedEntities.Select(entity => entity.GetComponent<HudComponent>());
                DisplayMultiSelectionGui(hudComps);
            }
        }

        private void DisplayNoGui()
        {
            _centerPanel.Children.Clear();
            _rightPanel.Children.Clear();
        }

        // When only one unit or building is selected
        private void DisplaySingleSelectionGui(HudComponent component)
        {
            if (IsHudLoaded(component)) return;

            DisplayNoGui();

            if (component.Action == null || component.Info == null) return;
            _centerPanel.Children.Add(component.Info);
            _rightPanel.Children.Add(component.Action);
        }

        // When multiple units are selected
        private void DisplayMultiSelectionGui(IEnumerable<HudComponent> components)
        {
            DisplayNoGui();
            var portraits = components.Select(comp => comp.Portrait);
        }

        private bool IsHudLoaded(HudComponent component)
        {
            return _centerPanel.Children.Count == 1 && _rightPanel.Children.Count == 1
                && _centerPanel.Children.Contains(component.Info) && _rightPanel.Children.Contains(component.Action);
        }

        private void UpdateLabels(Player player)
        {
            var resourceComponent = player.GetComponent<ResourceComponent>();

            int maxPopulation = PlayerHelper.GetMaxPopulation(player);
            string population = resourceComponent.Population + "/" + maxPopulation;

            _playerWood.Content = resourceComponent.Wood;
            _playerGold.Content = resourceComponent.Gold;
            _playerPopulation.Content = population;
        }
    }
}
