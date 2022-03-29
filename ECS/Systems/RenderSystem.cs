using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.IO;

namespace Revolution.ECS.Systems
{
    public class RenderSystem : ISystem
    {
        private Canvas canvas;
        private HashSet<FrameworkElement> entities;

        public RenderSystem(Canvas canvas)
        {
            this.canvas = canvas;
            this.entities = new HashSet<FrameworkElement>();
        }

        public void Update(int deltaMs)
        {
            foreach (var entity in EntityManager.GetEntities())
            {
                var posComp = entity.GetComponent<PositionComponent>();
                var sizeComp = entity.GetComponent<SizeComponent>();

                if (entity is Villager)
                {
                    Console.WriteLine("asd");
                }
            
                if (posComp != null && sizeComp != null)
                {
                    foreach (var component in entity.GetComponents())
                    {
                        Type componentType = component.GetType();
                        if (componentType == typeof(RenderComponent) || 
                            componentType.IsSubclassOf(typeof(RenderComponent)))
                        {
                            var renderComp = (RenderComponent)component;
                            var renderable = renderComp.Renderable;

                            if (!entities.Contains(renderable))
                            {
                                renderable.Width = sizeComp.Width;
                                renderable.Height = sizeComp.Height;

                                canvas.Children.Add(renderable);
                                Canvas.SetLeft(renderable, posComp.X);
                                Canvas.SetTop(renderable, posComp.Y);
                                Panel.SetZIndex(renderable, renderComp.ZIndex);

                                entities.Add(renderable);
                                entity.DestroyEvent += OnEntityDestroyed;
                            }
                        }
                    }
                }
            }
        }

        private void OnEntityDestroyed(object? sender, Entity e)
        {
            // Remove from local cache to save space
            e.DestroyEvent -= OnEntityDestroyed;
            foreach (var comp in e.GetComponents())
            {
                Type componentType = comp.GetType();
                if (componentType == typeof(RenderComponent) ||
                            componentType.IsSubclassOf(typeof(RenderComponent)))
                {
                    var renderComp = (RenderComponent)(comp);
                    entities.Remove(renderComp.Renderable);
                }
            }
        }
    }
}