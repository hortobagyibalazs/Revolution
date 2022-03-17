using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private HashSet<Entity> entities;

        public RenderSystem(Canvas canvas)
        {
            this.canvas = canvas;
            this.entities = new HashSet<Entity>();
        }

        public void Update(int deltaMs)
        {
            foreach (var entity in EntityManager.GetEntities())
            {
                var posComp = entity.GetComponent<PositionComponent>();
                var renderComp = entity.GetComponent<RenderComponent>();
                var sizeComp = entity.GetComponent<SizeComponent>();

                if (posComp != null && renderComp != null && sizeComp != null)
                {
                    var renderable = renderComp.Renderable;

                    if (!entities.Contains(entity))
                    {
                        canvas.Children.Add(renderable);
                        entities.Add(entity);
                    }
                }
            }
        }
    }
}