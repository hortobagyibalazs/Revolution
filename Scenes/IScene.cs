using System;
using System.Windows.Controls;

namespace Revolution.Scenes
{
    public interface IScene
    {
        SceneManager? Manager { get; protected internal set; }

        Control Content { get; }
        
        void OnEnter();
        void OnPause();
        void OnResume();
        
        // Don't forget to manually destroy scene related entities here
        void OnExit(EventHandler onFinish);
    }
}