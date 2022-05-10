using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using Revolution.ECS.Components;
using Revolution.HUD.Events;
using Revolution.IO;
using Revolution.Misc;
using Revolution.Misc.Cheats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Revolution.ECS.Systems
{
    public class CheatcodeSystem : ISystem, IRecipient<AddCheatcodeEvent>
    {
        private HashSet<CheatCode> Cheatcodes;
        private IMessenger _messenger = Ioc.Default.GetService<IMessenger>();

        public CheatcodeSystem(FrameworkElement root)
        {
            Cheatcodes = new HashSet<CheatCode>();
            root.PreviewKeyDown += Root_KeyDown;
            root.MouseDown += delegate
            {
                root.Focus();
                Keyboard.Focus(root);
            };

            Cheatcodes.Add(CreateHesoyam());
            Cheatcodes.Add(CreateJackhammer());
        }

        private void Root_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            foreach(var code in Cheatcodes)
            {
                code.ProcessKey(e.Key);
            }
        }

        public void Receive(AddCheatcodeEvent message)
        {
            Cheatcodes.Add(message.Cheatcode);
        }

        public void Update(int deltaMs)
        {

        }

        private CheatCode CreateHesoyam()
        {
            var code = new CheatCode(new System.Windows.Input.Key[] {
                    System.Windows.Input.Key.H,
                    System.Windows.Input.Key.E,
                    System.Windows.Input.Key.S,
                    System.Windows.Input.Key.O,
                    System.Windows.Input.Key.Y,
                    System.Windows.Input.Key.A,
                    System.Windows.Input.Key.M
                });

            code.CodeEntered += delegate
            {
                AddResourcesToPlayer();
            };

            return code;
        }

        private void AddResourcesToPlayer()
        {
            var player = PlayerHelper.GetGuiControlledPlayer();
            var resourceComp = player.GetComponent<ResourceComponent>();
            resourceComp.Wood += 500;
            resourceComp.Gold += 500;
            _messenger.Send(new ShowToastEvent(GlobalStrings.CheatActivated));
        }

        private CheatCode CreateJackhammer()
        {
            var code = new CheatCode(new System.Windows.Input.Key[] {
                    Key.J,
                    Key.A,
                    Key.C,
                    Key.K,
                    Key.H,
                    Key.A,
                    Key.M,
                    Key.M,
                    Key.E,
                    Key.R
                });

            code.CodeEntered += delegate
            {
                
                TurnOnFastBuilding();
            };

            return code;
        }

        private void TurnOnFastBuilding()
        {
            GlobalConfig.HouseBuildPoints = 1;
            GlobalConfig.BarracksBuildPoints = 1;
            _messenger.Send(new ShowToastEvent(GlobalStrings.CheatActivated));
        }
    }
}
