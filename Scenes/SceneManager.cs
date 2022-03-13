using System;
using System.Collections.Generic;
using System.Linq;

namespace Revolution.Scenes
{
    public class SceneManager
    {
        private Stack<IScene> _scenes;

        public IEnumerable<IScene> Scenes => _scenes.AsEnumerable();

        public IScene? Scene
        {
            get
            {
                if (_scenes.Count > 0)
                {
                    return _scenes.Peek();
                }

                return null;
            }
        }

        public event EventHandler<IScene> ScenePushed;
        public event EventHandler<IScene> ScenePopped;

        public SceneManager()
        {
            _scenes = new Stack<IScene>();
        }

        public void Push(IScene scene)
        {
            scene.Manager = this;
            Scene?.OnPause();
            _scenes.Push(scene);
            Scene?.OnEnter();
            ScenePushed?.Invoke(this, scene);
        }

        public IScene? Pop()
        {
            try
            {
                Scene?.OnExit(delegate(object? s, EventArgs args)
                {
                    var popped = _scenes.Pop();
                    ScenePopped?.Invoke(this, popped);
                    Scene?.OnResume();
                    popped.Manager = null;
                });

                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}