using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Revolution.ECS.Systems
{
    public class PlayerInputSystem : ISystem
    {
        private HashSet<PlayerInputComponent> _components = new HashSet<PlayerInputComponent>();
        private Canvas _canvas;

        public PlayerInputSystem(Canvas canvas)
        {
            _canvas = canvas;
        }

        public void Update(int deltaMs)
        {
            foreach (var entity in EntityManager.GetEntities())
            {
                var inputComp = entity.GetComponent<PlayerInputComponent>();
                if (inputComp != null && !_components.Contains(inputComp))
                {
                    _components.Add(inputComp);

                    if (inputComp.MouseMoveEventHandler != null)
                        _canvas.PreviewMouseMove += inputComp.MouseMoveEventHandler;

                    if (inputComp.MouseButtonUpEventHandler != null)
                    {
                        _canvas.PreviewMouseLeftButtonUp += inputComp.MouseButtonUpEventHandler;
                        _canvas.PreviewMouseRightButtonUp += inputComp.MouseButtonUpEventHandler;
                    }

                    if (inputComp.MouseButtonDownEventHandler != null)
                    {
                        _canvas.PreviewMouseLeftButtonDown += inputComp.MouseButtonDownEventHandler;
                        _canvas.PreviewMouseRightButtonDown += inputComp.MouseButtonDownEventHandler;
                    }

                    entity.DestroyEvent += delegate (object sender, Entity e)
                    {
                        var comp = e.GetComponent<PlayerInputComponent>();

                        if (comp.MouseMoveEventHandler != null)
                            _canvas.PreviewMouseMove -= comp.MouseMoveEventHandler;

                        if (comp.MouseButtonUpEventHandler != null)
                        {
                            _canvas.PreviewMouseLeftButtonUp -= comp.MouseButtonUpEventHandler;
                            _canvas.PreviewMouseRightButtonUp -= comp.MouseButtonUpEventHandler;
                        }

                        if (comp.MouseButtonDownEventHandler != null)
                        {
                            _canvas.PreviewMouseLeftButtonDown -= comp.MouseButtonDownEventHandler;
                            _canvas.PreviewMouseRightButtonDown -= comp.MouseButtonDownEventHandler; _components.Remove(inputComp);
                        }

                        _components.Remove(comp);
                    };
                }
            }
        }
    }
}
