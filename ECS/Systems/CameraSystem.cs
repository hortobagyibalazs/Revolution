using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.IO;

namespace Revolution.ECS.Systems
{
    public class CameraSystem : ISystem
    {
        private ScrollViewer scrollViewer;
        private readonly Canvas canvas;
        
        public int CameraSpeed { get; set; }
        public int BorderDistance { get; set; }
        public bool KeyboardControlsEnabled { get; set; }

        public CameraSystem(ScrollViewer canvasViewer, Canvas mainCanvas)
        {
            scrollViewer = canvasViewer;
            canvas = mainCanvas;

            CameraSpeed = GlobalConfig.TileSize / 4;
            BorderDistance = 30;
            KeyboardControlsEnabled = false;
        }


        public void Update(int deltaMs)
        {
            foreach (var entity in EntityManager.GetEntities())
            {
                var cameraComp = entity.GetComponent<CameraComponent>();
                if (cameraComp != null)
                {
                    var offsetX = 0;
                    var offsetY = 0;

                    if (MouseInUpperSegment() || (Keyboard.IsKeyDown(Key.W) && KeyboardControlsEnabled))
                    {
                        offsetY = -CameraSpeed;
                    }
                    else if (MouseInLowerSegment()|| (Keyboard.IsKeyDown(Key.S) && KeyboardControlsEnabled))
                    {
                        offsetY = CameraSpeed;
                    }

                    if (MouseInLeftSegment() || (Keyboard.IsKeyDown(Key.A) && KeyboardControlsEnabled))
                    {
                        offsetX = -CameraSpeed;
                    }
                    else if (MouseInRightSegment() || (Keyboard.IsKeyDown(Key.D) && KeyboardControlsEnabled))
                    {
                        offsetX = CameraSpeed;
                    }

                    var newX = cameraComp.X + offsetX;
                    var newY = cameraComp.Y + offsetY;

                    if (newX >= 0 && newX <= canvas.Width - scrollViewer.ViewportWidth)
                    {
                        cameraComp.SnapTo(newX, cameraComp.Y);
                    }

                    if (newY >= 0 && newY <= canvas.Height - scrollViewer.ViewportHeight)
                    {
                        cameraComp.SnapTo(cameraComp.X, newY);
                    }

                    scrollViewer.ScrollToHorizontalOffset(cameraComp.X);
                    scrollViewer.ScrollToVerticalOffset(cameraComp.Y);

                    return;
                }
            }
        }

        private bool MouseInUpperSegment()
        {
            return Mouse.GetPosition(scrollViewer).Y < BorderDistance;
        }

        private bool MouseInLowerSegment()
        {
            return Mouse.GetPosition(scrollViewer).Y > scrollViewer.ViewportHeight - BorderDistance;
        }

        private bool MouseInLeftSegment()
        {
            return Mouse.GetPosition(scrollViewer).X < BorderDistance;
        }

        private bool MouseInRightSegment()
        {
            return Mouse.GetPosition(scrollViewer).X > scrollViewer.ViewportWidth - BorderDistance;
        }
    }
}