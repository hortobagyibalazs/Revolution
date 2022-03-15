using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Key = Avalonia.Remote.Protocol.Input.Key;

namespace Revolution.IO
{
    // TODO : Put cheat code processing into its own manager file  
    public class Keyboard : IKeyboard
    {
        private HashSet<Key> pressed;
        private HashSet<CheatCode> cheatCodes;

        public Keyboard(Window window)
        {
            pressed = new HashSet<Key>();
            cheatCodes = new HashSet<CheatCode>();

            window.KeyUp += OnKeyUp;
            window.KeyDown += OnKeyDown;
        }

        private void OnKeyUp(object? sender, KeyEventArgs e)
        {
            e.Handled = true;
            
            Key result;
            Enum.TryParse(e.Key.ToString(), out result);
            pressed.Remove(result);

            foreach (CheatCode code in cheatCodes)
            {
                code.ProcessKey(result);
            }
        }

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            e.Handled = true;
            
            Key result;
            Enum.TryParse(e.Key.ToString(), out result);
            pressed.Add(result);
        }
        
        public bool IsDown(Key key)
        {
            return pressed.Contains(key);
        }

        public void FireKeyUp(Key key)
        {
            
        }

        public void FireKeyDown(Key key)
        {
            
        }

        public bool IsShortcutPressed(Key key1, Key key2)
        {
            throw new NotImplementedException();
        }

        public bool IsShortcutPressed(Key key1, Key key2, Key key3)
        {
            throw new NotImplementedException();
        }
        
        public void RegisterCheatCode(CheatCode cheatCode)
        {
            cheatCodes.Add(cheatCode);
        }

        public void UnregisterCheatCode(CheatCode cheatCode)
        {
            cheatCodes.Remove(cheatCode);
        }
    }
}