using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Key = Avalonia.Remote.Protocol.Input.Key;

namespace Revolution.IO
{
    public class Keyboard : IKeyboard
    {
        private HashSet<Key> pressed;
        private Window window;

        public static readonly Keyboard Instance = new Keyboard();
        private static bool _initialized;

        public static void Init(Window window)
        {
            if (_initialized) return;

            window.KeyUp += OnKeyUp;
            window.KeyDown += OnKeyDown;
            Instance.window = window;
            _initialized = true;
        }

        public static void CleanUp()
        {
            Instance.window.KeyUp -= OnKeyUp;
            Instance.window.KeyDown -= OnKeyDown;
            Instance.window = null;
            _initialized = false;
        }

        private Keyboard()
        {
            pressed = new HashSet<Key>();
        }

        private static void OnKeyUp(object? sender, KeyEventArgs e)
        {
            e.Handled = true;
            
            Key result;
            Enum.TryParse(e.Key.ToString(), out result);
            Instance.pressed.Remove(result);
        }

        private static void OnKeyDown(object? sender, KeyEventArgs e)
        {
            e.Handled = true;
            
            Key result;
            Enum.TryParse(e.Key.ToString(), out result);
            Instance.pressed.Add(result);
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
    }
}