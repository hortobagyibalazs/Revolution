using System;
using System.Windows.Controls;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.IO;

namespace Revolution.Scenes
{
    public class GameScene : IScene
    {
        private SceneManager? _manager;

        SceneManager? IScene.Manager
        {
            get => _manager;
            set => _manager = value;
        }

        public Canvas Canvas { get; }

        public void OnEnter()
        {
            
        }

        public void OnPause()
        {
            
        }

        public void OnResume()
        {
            
        }

        public void OnExit(EventHandler onFinish)
        {
            
        }

        public GameScene(Canvas canvas)
        {
            Canvas = canvas;

            EntityManager.CreateEntity<Camera>();
            var mapData = MapLoader.LoadFromFile(@"Assets\tileset.png", @"Assets\test.tmx");
            canvas.Width = mapData.Dimension.X * GlobalConfig.TileSize;
            canvas.Height = mapData.Dimension.Y * GlobalConfig.TileSize;
        }
    }
}