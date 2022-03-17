using System;
using Avalonia.Controls;
using Avalonia.Input;

namespace Revolution.IO
{
    public class Mouse : IMouse
    {
        public int CursorX { get; private set; }
        public int CursorY { get; private set; }

        private int initialClickX;
        private int initialClickY;

        public static readonly Mouse Instance = new Mouse();
        private static bool _initialized;
        private Window? window;

        private Mouse()
        {
            
        }

        public static void Init(Window window)
        {
            if (_initialized) return;

            window.PointerMoved += WindowOnPointerMoved;
            window.PointerPressed += WindowOnPointerPressed;
            window.PointerReleased += WindowOnPointerReleased;
            Instance.window = window;
            _initialized = true;
        }

        public static void CleanUp()
        {
            Instance.window.PointerMoved -= WindowOnPointerMoved;
            Instance.window.PointerPressed -= WindowOnPointerPressed;
            Instance.window.PointerReleased -= WindowOnPointerReleased;
            Instance.window = null;
            _initialized = false;
        }

        private static void WindowOnPointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            Instance.OnMouseButtonRelease(e.InitialPressMouseButton, Instance.initialClickX, Instance.initialClickY,
                (int) e.GetPosition(Instance.window).X, (int) e.GetPosition(Instance.window).Y);
        }

        private static void WindowOnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            Instance.initialClickX = (int) e.GetPosition(Instance.window).X;
            Instance.initialClickY = (int) e.GetPosition(Instance.window).Y;
        }

        private static void WindowOnPointerMoved(object? sender, PointerEventArgs e)
        {
            Instance.CursorX = (int) e.GetPosition(Instance.window).X;
            Instance.CursorY = (int) e.GetPosition(Instance.window).Y;
        }

        public void OnMouseButtonPress(MouseButton button, int x, int y)
        {
            throw new System.NotImplementedException();
        }

        public void OnMouseButtonRelease(MouseButton button, int initialClickX, int initialClickY, int releaseX,
            int releaseY)
        {
            throw new System.NotImplementedException();
        }

        public void OnMouseMove(int x, int y)
        {
            throw new System.NotImplementedException();
        }
    }
}