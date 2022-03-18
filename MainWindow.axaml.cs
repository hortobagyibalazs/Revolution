using System;
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using Revolution.ECS.Systems;
using Revolution.IO;
using Revolution.Scenes;

namespace Revolution
{
    public partial class MainWindow : Window
    {
        private int fps = 60;
        private long lastUpdate;
        private DispatcherTimer timer;
        
        private SceneManager sceneManager;
        private SystemManager systemManager;
        
        public MainWindow()
        {
            InitializeComponent();
            //WindowState = WindowState.FullScreen;

            // Setup timer
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / fps);
            timer.Tick += UpdateScenes;

            // Setup scene management
            sceneManager = new SceneManager();

            sceneManager.ScenePushed += ScenePushed;
            sceneManager.ScenePopped += ScenePopped;

            sceneManager.Push(new GameScene(MainCanvas));
            
            // Setup entity-component system
            systemManager = new SystemManager();
            systemManager.RegisterSystem(new RenderSystem(MainCanvas));
            systemManager.RegisterSystem(new CameraSystem(CanvasViewer, MainCanvas));
            
            Keyboard.Init(this);
            Mouse.Init(this);

            // Start timer
            lastUpdate = Environment.TickCount;
            timer.Start();
        }

        private void UpdateScenes(object? sender, EventArgs args)
        {
            // Update every system
            int deltaMs = (int) (Environment.TickCount - lastUpdate);
            lastUpdate = Environment.TickCount;
            systemManager.Update(deltaMs);
        }

        private void ScenePushed(object? sender, IScene e)
        {
            //MainCanvas.Children.Add(e.Canvas);
        }

        private void ScenePopped(object? sender, IScene e)
        {
            //MainCanvas.Children.Remove(e.Canvas);
        }
    }
}