using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.ECS.Systems;
using Revolution.IO;

namespace Revolution.Scenes
{
    public class GameScene : IScene
    {
        private SceneManager? _manager;

        private int fps = 60;
        private long lastUpdate;
        private DispatcherTimer timer;

        private SystemManager systemManager;

        SceneManager? IScene.Manager
        {
            get => _manager;
            set => _manager = value;
        }

        public Control Content { get; }

        public void OnEnter()
        {
            
        }

        public void OnPause()
        {
            timer.Stop();
        }

        public void OnResume()
        {
            timer.Start();
        }

        public void OnExit(EventHandler onFinish)
        {
            timer.Stop();
        }

        public GameScene(FrameworkElement Root)
        {
            GameUiControl contentHolder = new GameUiControl();
            var scrollViewer = contentHolder.CanvasViewer;
            var canvas = contentHolder.MainCanvas;
            Content = contentHolder;

            // Setup timer
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / fps);
            timer.Tick += UpdateSystems;

            var mapData = MapLoader.LoadFromFile(@"", @"Assets\map2.tmx");

            // Setup entity-component system
            systemManager = new SystemManager();
            systemManager.RegisterSystem(new RenderSystem(canvas));
            systemManager.RegisterSystem(new CameraSystem(scrollViewer, canvas, Root));
            systemManager.RegisterSystem(new BuildingSystem(scrollViewer));
            systemManager.RegisterSystem(new MovementSystem());
            systemManager.RegisterSystem(new SelectionSystem(canvas, scrollViewer));
            systemManager.RegisterSystem(new MinimapSystem(contentHolder.Minimap, mapData, canvas, scrollViewer));
            systemManager.RegisterSystem(new SpriteAnimationSystem());
            systemManager.RegisterSystem(new MapSystem(mapData));
            systemManager.RegisterSystem(new HudSystem(contentHolder.InfoHud, contentHolder.ActionHud));
            systemManager.RegisterSystem(new TooltipSystem(contentHolder.Tooltip));
            systemManager.RegisterSystem(new ToastMessageSystem(contentHolder.MessageLabel));
            systemManager.RegisterSystem(new SpawnerSystem(mapData));
            systemManager.RegisterSystem(new CheatcodeSystem(canvas));

            // Start timer
            lastUpdate = Environment.TickCount;
            timer.Start();


            // Set canvas size
            canvas.Width = mapData.Dimension.X * GlobalConfig.TileSize;
            canvas.Height = mapData.Dimension.Y * GlobalConfig.TileSize;

            // Add entities
            EntityManager.CreateEntity<Camera>();
            Player player = EntityManager.CreateEntity<Player>();
            player.IsGuiControlled = true;
        }

        private void UpdateSystems(object? sender, EventArgs e)
        {
            // Update every system
            int deltaMs = (int)(Environment.TickCount - lastUpdate);
            lastUpdate = Environment.TickCount;
            systemManager.Update(deltaMs);
        }
    }
}