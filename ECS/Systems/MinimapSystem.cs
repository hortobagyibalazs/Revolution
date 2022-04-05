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
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Revolution.ECS.Systems
{
    public class MinimapSystem : ISystem
    {
        private Canvas minimapCanvas;
        private MapData mapData;

        private RenderTargetBitmap bitmap = null;
        private Image tileMapImage = null;

        public MinimapSystem(Canvas minimap, MapData mapData)
        {
            minimapCanvas = minimap;
            this.mapData = mapData;

            minimap.SizeChanged += Minimap_SizeChanged;
        }

        private void Minimap_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            bitmap = null;
            minimapCanvas.Children.Clear();
        }

        public void Update(int deltaMs)
        {
            DrawingVisual visual = null;
            DrawingContext drawingContext = null;

            if (bitmap == null && minimapCanvas.ActualWidth != double.NaN)
            {
                bitmap = new RenderTargetBitmap(
                    (int) minimapCanvas.ActualWidth, (int)minimapCanvas.ActualHeight, 96, 96, PixelFormats.Pbgra32
                );
                var img = new Image();
                img.Source = bitmap;
                img.Width = minimapCanvas.ActualWidth;
                img.Height = minimapCanvas.ActualHeight;
                minimapCanvas.Children.Add(img);
                Canvas.SetLeft(img, 0);
                Canvas.SetTop(img, 0);

                visual = new DrawingVisual();
                drawingContext = visual.RenderOpen();
            }

            foreach (var entity in EntityManager.GetEntities())
            {
                var gameMapObjectComp = entity.GetComponent<GameMapObjectComponent>();
                var minimapComp = entity.GetComponent<MinimapComponent>();

                if (gameMapObjectComp != null && minimapComp != null)
                {
                    if (drawingContext != null && minimapCanvas.ActualWidth != double.NaN)
                    {
                        double x = gameMapObjectComp.X * (minimapCanvas.ActualWidth / mapData.Dimension.X);
                        double y = gameMapObjectComp.Y * (minimapCanvas.ActualHeight / mapData.Dimension.Y);
                        double width = gameMapObjectComp.Width * (minimapCanvas.ActualWidth / mapData.Dimension.X);
                        double height = gameMapObjectComp.Height * (minimapCanvas.ActualHeight / mapData.Dimension.Y);

                        drawingContext.DrawRectangle(minimapComp.Background, null, new Rect(x, y, width, height));
                    }
                }
            }

            if (bitmap != null && visual != null && drawingContext != null)
            {
                drawingContext.Close();
                bitmap.Render(visual);
            }
        }

        private void OnEntityDestroyed(object? sender, Entity e)
        {
            // Remove from local cache to save space
            e.DestroyEvent -= OnEntityDestroyed;
            minimapCanvas.Children.Clear();
        }
    }
}
