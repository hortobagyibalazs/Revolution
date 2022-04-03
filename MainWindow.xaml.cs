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
        private SceneManager sceneManager;

        public MainWindow()
        {
            InitializeComponent();

            // Setup scene management
            sceneManager = new SceneManager();

            sceneManager.ScenePushed += ScenePushed;
            sceneManager.ScenePopped += ScenePopped;

            sceneManager.Push(new GameScene(Root));
        }


        private void ScenePushed(object? sender, IScene e)
        {
            Root.Children.Add(e.Content);
        }

        private void ScenePopped(object? sender, IScene e)
        {
            Root.Children.Remove(e.Content);
        }
    }
}
