using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.IO;
using Key = Avalonia.Remote.Protocol.Input.Key;

namespace Revolution.ECS.Systems
{
    public class CameraSystem : ISystem
    {
        private ScrollViewer scrollViewer;
        private Canvas canvas;
        
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

                    if (MouseInUpperSegment() || (Keyboard.Instance.IsDown(Key.W) && KeyboardControlsEnabled))
                    {
                        offsetY = -CameraSpeed;
                    }
                    else if (MouseInLowerSegment()|| (Keyboard.Instance.IsDown(Key.S) && KeyboardControlsEnabled))
                    {
                        offsetY = CameraSpeed;
                    }

                    if (MouseInLeftSegment() || (Keyboard.Instance.IsDown(Key.A) && KeyboardControlsEnabled))
                    {
                        offsetX = -CameraSpeed;
                    }
                    else if (MouseInRightSegment() || (Keyboard.Instance.IsDown(Key.D) && KeyboardControlsEnabled))
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
            return Mouse.Instance.CursorY < BorderDistance;
        }

        private bool MouseInLowerSegment()
        {
            return Mouse.Instance.CursorY > scrollViewer.Viewport.Height - BorderDistance;
        }

        private bool MouseInLeftSegment()
        {
            return Mouse.Instance.CursorX < BorderDistance;
        }

        private bool MouseInRightSegment()
        {
            return Mouse.Instance.CursorX > scrollViewer.Viewport.Width - BorderDistance;
        }
    }
}