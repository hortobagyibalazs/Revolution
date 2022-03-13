using System;
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using Revolution.Scenes;

namespace Revolution
{
    public partial class MainWindow : Window
    {
        private int fps = 60;
        
        private DispatcherTimer timer;
        private SceneManager sceneManager;
        
        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(1000 / fps);
            timer.Tick += UpdateScenes;

            sceneManager = new SceneManager();

            sceneManager.ScenePushed += ScenePushed;
            sceneManager.ScenePopped += ScenePopped;
            
            sceneManager.Push(new GameScene());

            timer.Start();
        }

        private void UpdateScenes(object? sender, EventArgs args)
        {
            foreach (var scene in sceneManager.Scenes)
            {
                scene.OnUpdate(0);
            }
        }

        private void ScenePushed(object? sender, IScene e)
        {
            MainCanvas.Children.Add(e.Canvas);
        }

        private void ScenePopped(object? sender, IScene e)
        {
            MainCanvas.Children.Remove(e.Canvas);
        }
    }
}