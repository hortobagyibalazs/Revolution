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
            //PathFinderSystem finder = new PathFinderSystem(new GridComponent(25, 25, 1));
            //finder.UpdateCell(3, 4, -2);
            //finder.UpdateCell(21, 5, -3);
            //Debug.WriteLine("===============< START >===============");
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //finder.PathFinding();
            //sw.Stop();
            //Debug.WriteLine(sw.ElapsedMilliseconds);
            //Debug.WriteLine("===============< END >===============");

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
            systemManager.RegisterSystem(new PathFinderSystem(new GridComponent((int) mapData.Dimension.X, (int) mapData.Dimension.Y, 1)));

            // Start timer
            lastUpdate = Environment.TickCount;
            timer.Start();

            EntityManager.CreateEntity<Camera>();
            canvas.Width = mapData.Dimension.X * GlobalConfig.TileSize;
            canvas.Height = mapData.Dimension.Y * GlobalConfig.TileSize;
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