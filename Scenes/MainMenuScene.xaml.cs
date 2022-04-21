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

namespace Revolution.Scenes
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenuScene : UserControl, IScene
    {
        public MainMenuScene()
        {
            InitializeComponent();
        }

        private SceneManager manager;
        SceneManager? IScene.Manager { get => manager; set => manager = value; }

        Control IScene.Content => this;
        private void Button_Exit(object sender, RoutedEventArgs e)
        {
            var asnwer = MessageBox.Show("Are you sure that you want to exit the game?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (asnwer.Equals(MessageBoxResult.Yes))
            {
                manager.Pop();
            }
        }
        void IScene.OnEnter()
        {
            
        }

        void IScene.OnExit(EventHandler onFinish)
        {
            throw new NotImplementedException();
        }

        void IScene.OnPause()
        {
            
        }

        void IScene.OnResume()
        {
            
        }
    }
}
