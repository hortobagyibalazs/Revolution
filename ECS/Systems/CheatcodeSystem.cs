using Microsoft.Toolkit.Mvvm.Messaging;
using Revolution.IO;
using Revolution.Misc.Cheats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Revolution.ECS.Systems
{
    public class CheatcodeSystem : ISystem, IRecipient<AddCheatcodeEvent>
    {
        private HashSet<CheatCode> Cheatcodes;

        public CheatcodeSystem(FrameworkElement root)
        {
            Cheatcodes = new HashSet<CheatCode>();
            root.KeyDown += Root_KeyDown;
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
    }
}
