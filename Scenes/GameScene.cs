using System;
using Avalonia.Controls;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;

namespace Revolution.Scenes
{
    public class GameScene : IScene
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

        public GameScene(Canvas canvas)
        {
            Canvas = canvas;
            Canvas.Width = 1200;
            Canvas.Height = 1000;

            EntityManager.CreateEntity<Camera>();
        }
    }
}