using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.IO;

namespace Revolution.ECS.Systems
{
    public class RenderSystem : ISystem
    {
        private Canvas canvas;
        private HashSet<FrameworkElement> renderables; // FrameworkElements currently added to the canvas

        public RenderSystem(Canvas canvas)
        {
            this.canvas = canvas;
            this.renderables = new HashSet<FrameworkElement>();
        }

        public void Update(int deltaMs)
        {
            foreach (var entity in EntityManager.GetEntities())
            {
                var entities = EntityManager.GetEntities();

                var posComp = entity.GetComponent<PositionComponent>();
                var sizeComp = entity.GetComponent<SizeComponent>();
                var directionComp = entity.GetComponent<DirectionComponent>();
            
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

                            if (!renderables.Contains(renderable))
                            {
                                if (directionComp != null)
                                {
                                    directionComp.PropertyChanged += delegate
                                    {
                                        ApplyRenderTransform(directionComp, renderable);
                                    };
                                }

                                renderable.Width = sizeComp.Width;
                                renderable.Height = sizeComp.Height;

                                canvas.Children.Add(renderable);
                                Canvas.SetLeft(renderable, posComp.X);
                                Canvas.SetTop(renderable, posComp.Y);
                                Panel.SetZIndex(renderable, renderComp.ZIndex);

                                renderables.Add(renderable);
                                ApplyRenderTransform(directionComp, renderable);
                                entity.DestroyEvent += OnEntityDestroyed;
                            }
                        }
                    }
                }
            }
        }

        private void ApplyRenderTransform(DirectionComponent directionComp, FrameworkElement renderable)
        {
            if (directionComp == null) return;

            int scale = -1;
            if (directionComp.Direction == Direction.Right)
            {
                scale = 1;
            }

            renderable.RenderTransform = new ScaleTransform() 
            { 
                ScaleX = scale, 
                CenterX = renderable.ActualWidth / 2 
            };
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
                    renderables.Remove(renderComp.Renderable);
                    canvas.Children.Remove(renderComp.Renderable);
                }
            }
        }
    }
}