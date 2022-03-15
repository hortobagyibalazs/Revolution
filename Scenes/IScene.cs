using System;
using Avalonia.Controls;

namespace Revolution.Scenes
{
    public interface IScene
    {
        SceneManager? Manager { get; protected internal set; }

        Canvas Canvas { get; }
        
        void OnEnter();
        void OnPause();
        void OnResume();
        
        // Don't forget to manually destroy scene related entities here
        void OnExit(EventHandler onFinish);
    }
}