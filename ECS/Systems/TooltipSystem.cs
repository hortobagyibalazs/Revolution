using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using Revolution.ECS.Entities;
using Revolution.HUD.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Revolution.ECS.Systems
{
    public class TooltipSystem : ISystem, IRecipient<ShowTooltipEvent>, IRecipient<HideTooltipEvent>
    {
        private IMessenger _messenger = Ioc.Default.GetService<IMessenger>();

        private Panel _root;

        public TooltipSystem(Panel tooltipView)
        {
            _root = tooltipView;
            _messenger.Register<ShowTooltipEvent>(this);
            _messenger.Register<HideTooltipEvent>(this);
        }

        public void Receive(ShowTooltipEvent message)
        {
            if (_root.Children.Count >= 1 && _root.Children[0] == message.Drawable) return;

            _root.Children.Clear();
            _root.Children.Add(message.Drawable);
            _root.Visibility = Visibility.Visible;
        }

        public void Receive(HideTooltipEvent message)
        {
            _root.Children.Remove(message.Drawable);
            _root.Visibility = Visibility.Collapsed;
        }

        public void Update(int deltaMs)
        {
            
        }
    }
}
