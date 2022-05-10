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
using System.Windows.Input;
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
                CreateMinimap(ref visual, ref drawingContext);
            }

            foreach (var entity in EntityManager.GetEntities())
            {
                var gameMapObjectComp = entity.GetComponent<GameMapObjectComponent>();
                var minimapComp = entity.GetComponent<MinimapComponent>();

                var cameraComp = entity.GetComponent<CameraComponent>();

                if (minimapComp != null)
                {
                    if (drawingContext != null && minimapCanvas.ActualWidth != double.NaN)
                    {
                        minimapComp.Draw?.Invoke(this, new MinimapDrawEventArgs(drawingContext, minimapCanvas, mapData));
                    }
                }
                else if (cameraComp != null)
                {
                    UpdateCameraPos(cameraComp);
                    UpdateViewPortMarkerPos(cameraComp);
                }
            }

            if (bitmap != null && visual != null && drawingContext != null)
            {
                drawingContext.Close();
                bitmap.Render(visual);
            }
        }

        private void CreateMinimap(ref DrawingVisual visual, ref DrawingContext drawingContext)
        {
            minimapRect.Width = (int)minimapCanvas.ActualWidth * (scrollViewer.ActualWidth / gameCanvas.ActualWidth);
            minimapRect.Height = (int)minimapCanvas.ActualHeight * (scrollViewer.ActualHeight / gameCanvas.ActualHeight);

            bitmap = new RenderTargetBitmap(
                (int)minimapCanvas.ActualWidth, (int)minimapCanvas.ActualHeight, 96, 96, PixelFormats.Pbgra32
            );
            tileMapImage = new System.Windows.Controls.Image();
            tileMapImage.Source = bitmap;
            tileMapImage.Width = minimapCanvas.ActualWidth;
            tileMapImage.Height = minimapCanvas.ActualHeight;

            minimapCanvas.Children.Add(tileMapImage);
            minimapCanvas.Children.Add(minimapRect);

            Canvas.SetLeft(tileMapImage, 0);
            Canvas.SetTop(tileMapImage, 0);
            Canvas.SetZIndex(minimapRect, 1);

            visual = new DrawingVisual();
            drawingContext = visual.RenderOpen();
        }

        private void UpdateViewPortMarkerPos(CameraComponent cameraComp)
        {
            int x = (int)((cameraComp.X / gameCanvas.ActualWidth) * minimapCanvas.ActualWidth);
            int y = (int)((cameraComp.Y / gameCanvas.ActualHeight) * minimapCanvas.ActualHeight);
            Canvas.SetLeft(minimapRect, x);
            Canvas.SetTop(minimapRect, y);
        }

        private void UpdateCameraPos(CameraComponent cameraComp)
        {
            if ((Mouse.LeftButton != MouseButtonState.Pressed) || (!minimapCanvas.IsMouseOver))
            {
                return;
            }

            var mousePos = Mouse.GetPosition(minimapCanvas);
            int mouseX = (int) (mousePos.X - (minimapRect.ActualWidth / 2));
            int mouseY = (int) (mousePos.Y - (minimapRect.ActualHeight / 2));

            if (mouseX < 0)
            {
                mouseX = 0;
            }
            else if (mouseX > minimapCanvas.ActualWidth - minimapRect.ActualWidth)
            {
                mouseX = (int) (minimapCanvas.ActualWidth - minimapRect.ActualWidth);
            }

            if (mouseY < 0)
            {
                mouseY = 0;
            }
            else if (mouseY > minimapCanvas.ActualHeight - minimapRect.ActualHeight)
            {
                mouseY = (int) (minimapCanvas.ActualHeight - minimapRect.ActualHeight);
            }

            int cameraX = (int) ((mouseX / minimapCanvas.ActualWidth) * gameCanvas.ActualWidth);
            int cameraY = (int) ((mouseY / minimapCanvas.ActualHeight) * gameCanvas.ActualHeight);

            cameraComp.SnapTo(cameraX, cameraY);
        }
    }
}
