using System;
using Avalonia;
using Avalonia.Controls;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.IO;

namespace Revolution.ECS.Systems
{
    public class CameraSystem : ISystem
    {
        private ScrollViewer scrollViewer;
        private Canvas canvas;
        
        public int CameraSpeed { get; set; }

        public CameraSystem(ScrollViewer canvasViewer, Canvas mainCanvas)
        {
            scrollViewer = canvasViewer;
            canvas = mainCanvas;

            CameraSpeed = 10;
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

                    if (MouseInUpperSegment())
                    {
                        offsetY = -CameraSpeed;
                    }
                    else if (MouseInLowerSegment())
                    {
                        offsetY = CameraSpeed;
                    }

                    if (MouseInLeftSegment())
                    {
                        offsetX = -CameraSpeed;
                    }
                    else if (MouseInRightSegment())
                    {
                        offsetX = CameraSpeed;
                    }

                    var newX = cameraComp.X + offsetX;
                    var newY = cameraComp.Y + offsetY;

                    if (newX >= 0 && newX <= canvas.Width - scrollViewer.Viewport.Width)
                    {
                        cameraComp.SnapTo(newX, cameraComp.Y);
                    }

                    if (newY >= 0 && newY <= canvas.Height - scrollViewer.Viewport.Height)
                    {
                        cameraComp.SnapTo(cameraComp.X, newY);
                    }

                    scrollViewer.Offset = new Vector(cameraComp.X, cameraComp.Y);
                    return;
                }
            }
        }

        private bool MouseInUpperSegment()
        {
            return Mouse.Instance.CursorY < 30;
        }

        private bool MouseInLowerSegment()
        {
            return Mouse.Instance.CursorY > scrollViewer.Viewport.Height - 30;
        }

        private bool MouseInLeftSegment()
        {
            return Mouse.Instance.CursorX < 30;
        }

        private bool MouseInRightSegment()
        {
            return Mouse.Instance.CursorX > scrollViewer.Viewport.Width - 30;
        }
    }
}