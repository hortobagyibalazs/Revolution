using System;
using System.Security.Cryptography;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.IO;

namespace Revolution.ECS.Systems
{
    public class RenderSystem : ISystem
    {
        private Canvas canvas;

        public RenderSystem(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public void Update(int deltaMs)
        {
            // TODO : Optimize
            canvas.Children.Clear();
            foreach (var entity in EntityManager.GetEntities())
            {
                var posComp = entity.GetComponent<PositionComponent>();
                var renderComp = entity.GetComponent<RenderComponent>();
                var sizeComp = entity.GetComponent<SizeComponent>();

                if (posComp != null && renderComp != null && sizeComp != null)
                {
                    var renderable = renderComp.Renderable;
                    renderable.Width = sizeComp.Width;
                    renderable.Height = sizeComp.Height;
                    canvas.Children.Add(renderable);
                    Canvas.SetLeft(renderable, posComp.X);
                    Canvas.SetTop(renderable, posComp.Y);
                }
            }
        }
    }
}