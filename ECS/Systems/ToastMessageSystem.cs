using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using Revolution.HUD.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Revolution.ECS.Systems
{
    public class ToastMessageSystem : ISystem, IRecipient<ShowToastEvent>
    {
        private IMessenger _messenger = Ioc.Default.GetService<IMessenger>();

        private Label _label;
        private long _startTime = Environment.TickCount;
        private long _timeout = 20000; // toast timeout in ms

        public ToastMessageSystem(Label label)
        {
            _label = label;
            _messenger.Register<ShowToastEvent>(this);
        }

        public void Receive(ShowToastEvent message)
        {
            _label.Content = message.Message;
            _label.Visibility = System.Windows.Visibility.Visible;
            _label.Opacity = 1;
            _startTime = Environment.TickCount;
        }

        public void Update(int deltaMs)
        {
            if (Environment.TickCount - _startTime < _timeout)
            {
                _label.Opacity -= 0.01;
            }
            else
            {
                _label.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
