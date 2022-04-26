using System;
using System.Collections.Generic;
using System.IO;
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

namespace Revolution.Scenes
{
    /// <summary>
    /// Interaction logic for MakerssCreditsScene.xaml
    /// </summary>
    public partial class MakersCreditsScene : UserControl, IScene
    {
        FrameworkElement framework { get; set; }
        public MakersCreditsScene(FrameworkElement frameworkElement)
        {
            this.framework = frameworkElement;
            InitializeComponent();
        }

        Control IScene.Content => this;

        private SceneManager manager;
        SceneManager? IScene.Manager { get => manager; set => manager = value; }
        public MakersCreditsScene()
        {
            InitializeComponent();
        }

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

        private void Button_RG_EasterEgg(object sender, RoutedEventArgs e)
        {
            MediaPlayer mediaPlayer = new MediaPlayer();
            var path = Path.Combine(@"\Assets\Musics\rick_astley_never_gonna_give_you_up_video.mp3");

            mediaPlayer.Open(new Uri(path, UriKind.Relative));
            mediaPlayer.Play();
        }
    }
}
