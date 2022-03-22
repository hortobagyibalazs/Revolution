using Avalonia.Input;

namespace Revolution.IO
{
    public interface IMouse
    {
        int CursorX { get; }
        int CursorY { get; }

        void OnMouseButtonPress(MouseButton button, int x, int y);
        void OnMouseButtonRelease(MouseButton button, int initialClickX, int initialClickY, int releaseX, int releaseY);
        void OnMouseMove(int x, int y);
        bool IsDown(MouseButton button);
    }
}