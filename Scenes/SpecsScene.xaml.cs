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
    /// Interaction logic for SpecsScene.xaml
    /// </summary>
    public partial class SpecsScene : UserControl , IScene 
    {
        FrameworkElement framework { get; set; }
        public SpecsScene(FrameworkElement frameworkElement)
        {
            this.framework = frameworkElement;
            InitializeComponent();
        }

        Control IScene.Content => this;

        private SceneManager manager;
        SceneManager? IScene.Manager { get => manager; set => manager = value; }

        void IScene.OnEnter()
        {
            
        }

        void IScene.OnPause()
        {

        }

        void IScene.OnResume()
        {

        }

        void IScene.OnExit(EventHandler onFinish)
        {
            onFinish.Invoke(null, null);
        }

        private void Button_RetunToMainMenu(object sender, RoutedEventArgs e)
        {
            manager.Pop();
        }
    }
}
