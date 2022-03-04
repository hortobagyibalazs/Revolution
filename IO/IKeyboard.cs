
using System;
using System.Collections.Generic;
using Avalonia.Remote.Protocol.Input;

namespace Revolution.IO
{
    public interface IKeyboard
    {
        bool IsDown(Key key);
        void FireKeyUp(Key key);
        void FireKeyDown(Key key);
        bool IsShortcutPressed(Key key1, Key key2);
        bool IsShortcutPressed(Key key1, Key key2, Key key3);
        void RegisterCheatCode(CheatCode cheatCode);
        void UnregisterCheatCode(CheatCode cheatCode);
    }
}