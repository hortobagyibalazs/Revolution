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
using System.Windows.Shapes;

namespace Revolution.ECS.Systems
{
    public class MinimapSystem : ISystem
    {
        private Canvas minimapCanvas;
        private MapData mapData;

        private Canvas gameCanvas;
        private ScrollViewer scrollViewer;

        private RenderTargetBitmap bitmap = null;
        private System.Windows.Controls.Image tileMapImage = null;

        private Rectangle minimapRect;

        public MinimapSystem(Canvas minimap, MapData mapData, Canvas gameCanvas, ScrollViewer canvasViewer)
        {
            minimapCanvas = minimap;
            this.mapData = mapData;

            this.gameCanvas = gameCanvas;
            this.scrollViewer = canvasViewer;

            minimap.SizeChanged += Minimap_SizeChanged;

            minimapRect = new Rectangle()
            {
                Stroke = Brushes.Yellow,
                StrokeThickness = 1
            };
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
                minimapRect.Width = (int) minimapCanvas.ActualWidth * (scrollViewer.ActualWidth / gameCanvas.ActualWidth);
                minimapRect.Height = (int) minimapCanvas.ActualHeight * (scrollViewer.ActualHeight / gameCanvas.ActualHeight);

                bitmap = new RenderTargetBitmap(
                    (int) minimapCanvas.ActualWidth, (int)minimapCanvas.ActualHeight, 96, 96, PixelFormats.Pbgra32
                );
                var img = new System.Windows.Controls.Image();
                img.Source = bitmap;
                img.Width = minimapCanvas.ActualWidth;
                img.Height = minimapCanvas.ActualHeight;

                minimapCanvas.Children.Add(img);
                minimapCanvas.Children.Add(minimapRect);

                Canvas.SetLeft(img, 0);
                Canvas.SetTop(img, 0);
                Canvas.SetZIndex(minimapRect, 1);

                visual = new DrawingVisual();
                drawingContext = visual.RenderOpen();
            }

            foreach (var entity in EntityManager.GetEntities())
            {
                var gameMapObjectComp = entity.GetComponent<GameMapObjectComponent>();
                var minimapComp = entity.GetComponent<MinimapComponent>();

                var cameraComp = entity.GetComponent<CameraComponent>();

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
                else if (cameraComp != null)
                {
                    int x = (int)((cameraComp.X / gameCanvas.ActualWidth) * minimapCanvas.ActualWidth);
                    int y = (int)((cameraComp.Y / gameCanvas.ActualHeight) * minimapCanvas.ActualHeight);
                    Canvas.SetLeft(minimapRect, x);
                    Canvas.SetTop(minimapRect, y);
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
