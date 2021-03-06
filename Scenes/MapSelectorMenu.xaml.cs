using Revolution.ECS.Models;
using Revolution.Misc;
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
    /// Interaction logic for MapSelectorMenu.xaml
    /// </summary>
    public partial class MapSelectorMenu : UserControl, IScene 
    {
        private Map selectedMap { get; set; }
        FrameworkElement framework { get; set; }
        public MapSelectorMenu(FrameworkElement frameworkElement)
        {
            this.framework = frameworkElement;
            InitializeComponent();
        }

        Control IScene.Content => this;

        private SceneManager manager;
        SceneManager? IScene.Manager { get => manager; set => manager = value; }

        void IScene.OnEnter()
        {
            MapMenuManager mapMenuManager = new MapMenuManager();
            //var maps = mapMenuManager.LoadMaps();
            var maps2 = mapMenuManager.CreateMaps();
            MapsView.ItemsSource = maps2;
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

        private void Button_PlaySelected(object sender, RoutedEventArgs e)
        {
            var idk = (Map)MapsView.SelectedItem;
            
            manager.Push(new GameScene(framework,idk));
        }

        private void MapsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
