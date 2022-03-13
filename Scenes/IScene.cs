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
        void OnExit(EventHandler onFinish);

        void OnUpdate(int deltaMs);
    }
}