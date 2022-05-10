using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using Revolution.Commands;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Revolution.HUD.EventHandlers
{
    public class TrooperInputHandler
    {
        private Entity _unit;
        private IMessenger _messenger = Ioc.Default.GetService<IMessenger>();

        public TrooperInputHandler(Entity unit)
        {
            _unit = unit;
        }

        public void RightButtonDownHandler(object sender, MouseEventArgs e)
        {
            bool? isSelected = _unit.GetComponent<SelectionComponent>().Selected;

            if (e.RightButton == MouseButtonState.Pressed && isSelected == true)
            {
                _messenger.Send(new MoveTrooperToCursorCommand(_unit));
            }
        }
    }
}
