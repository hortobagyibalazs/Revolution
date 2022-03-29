using Revolution.ECS.Systems;
using Revolution.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
            systemManager.RegisterSystem(new BuildingSystem(CanvasViewer));
            systemManager.RegisterSystem(new MovementSystem());
            systemManager.RegisterSystem(new SelectionSystem(MainCanvas, CanvasViewer));

            // Start timer
            lastUpdate = Environment.TickCount;
            timer.Start();
        }

        private void UpdateScenes(object? sender, EventArgs args)
        {
            // Update every system
            int deltaMs = (int)(Environment.TickCount - lastUpdate);
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
