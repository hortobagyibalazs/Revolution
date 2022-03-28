using System;
using System.Windows.Controls;

namespace Revolution.Scenes
{
    public class MainMenuScene : IScene
    {
        private SceneManager? _manager;

        SceneManager? IScene.Manager
        {
            get => _manager;
            set => _manager = value;
        }

        public Canvas Canvas { get; }
        public void OnEnter()
        {
            
        }

        public void OnPause()
        {
            
        }

        public void OnResume()
        {
            
        }

        public void OnExit(EventHandler onFinish)
        {
            
        }

        public MainMenuScene()
        {
            Canvas = new Canvas();
        }
    }
}