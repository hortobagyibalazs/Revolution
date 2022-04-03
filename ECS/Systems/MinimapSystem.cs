using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Revolution.ECS.Systems
{
    public class MinimapSystem : ISystem
    {
        private Canvas minimapCanvas;
        private IDictionary<Entity, FrameworkElement> renderables; // FrameworkElements currently added to the canvas
        private MapData mapData;

        public MinimapSystem(Canvas minimap, MapData mapData)
        {
            minimapCanvas = minimap;
            renderables = new Dictionary<Entity, FrameworkElement>();
            this.mapData = mapData;

            minimap.SizeChanged += Minimap_SizeChanged;
        }

        private void Minimap_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            renderables.Clear();
            minimapCanvas.Children.Clear();
        }

        public void Update(int deltaMs)
        {
            foreach (var entity in EntityManager.GetEntities())
            {
                var gameMapObjectComp = entity.GetComponent<GameMapObjectComponent>();
                var minimapComp = entity.GetComponent<MinimapComponent>();

                if (gameMapObjectComp != null && minimapComp != null)
                {
                    if (!renderables.ContainsKey(entity) && minimapCanvas.ActualWidth != double.NaN)
                    {
                        var lbl = new Label();  // blocks * (70px / 75bl)
                        lbl.Width = gameMapObjectComp.Width * (minimapCanvas.ActualWidth / mapData.Dimension.X);
                        lbl.Height = gameMapObjectComp.Height * (minimapCanvas.ActualHeight / mapData.Dimension.Y);
                        lbl.Background = minimapComp.Background;

                        minimapCanvas.Children.Add(lbl);
                        renderables.Add(entity, lbl);

                        var x = gameMapObjectComp.X * (minimapCanvas.ActualWidth / mapData.Dimension.X);
                        var y = gameMapObjectComp.Y * (minimapCanvas.ActualHeight / mapData.Dimension.Y);
                        Canvas.SetLeft(lbl, x);
                        Canvas.SetTop(lbl, y);
                    }
                }
            }
        }

        private void OnEntityDestroyed(object? sender, Entity e)
        {
            // Remove from local cache to save space
            e.DestroyEvent -= OnEntityDestroyed;
            var renderable = renderables[e];
            renderables.Remove(e);
            minimapCanvas.Children.Remove(renderable);
        }
    }
}
