using System;
using Avalonia.Controls;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;

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

        public GameScene()
        {
            Canvas = new Canvas();
            int tileSize = 32;

            for (int x = 0; x < 640 / tileSize; x++)
            {
                for (int y = 0; y < 480 / tileSize; y++)
                {
                    var entity = EntityManager.CreateEntity<TestEntity>();
                    entity.GetComponent<PositionComponent>().X = x * tileSize;
                    entity.GetComponent<PositionComponent>().Y = y * tileSize;
                }
            }
        }
    }
}