using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using Revolution.Commands;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.Misc;
using Revolution.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Revolution.HUD.EventHandlers
{
    public class VillagerInputHandler
    {
        private Villager _villager;
        private IMessenger _messenger = Ioc.Default.GetService<IMessenger>();

        public VillagerInputHandler(Villager villager)
        {
            _villager = villager;
        }

        public void RightButtonDownHandler(object sender, MouseEventArgs e)
        {
            bool? isSelected = _villager.GetComponent<SelectionComponent>().Selected;

            if (e.RightButton == MouseButtonState.Pressed && isSelected == true)
            {
                _messenger.Send(new MoveVillagerToCursorCommand(_villager));
            }
        }
    }
}
