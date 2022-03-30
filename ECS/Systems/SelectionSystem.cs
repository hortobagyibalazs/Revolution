using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Revolution.ECS.Systems
{
    public class SelectionSystem : ISystem
    {
        private Canvas Canvas;
        private ScrollViewer ScrollViewer;

        private Border SelectionRect;
        private bool Dragging;
        private Point? DragStart;

        public SelectionSystem(Canvas canvas, ScrollViewer scrollViewer)
        {
            Canvas = canvas;
            ScrollViewer = scrollViewer;

            SelectionRect = new Border()
            {
                BorderThickness = new System.Windows.Thickness(1),
                BorderBrush = new SolidColorBrush(Brushes.WhiteSmoke.Color),
                Visibility = Visibility.Hidden
            };
            canvas.Children.Add(SelectionRect);
        }

        public void Update(int deltaMs)
        {
            if (!Dragging && Mouse.LeftButton == MouseButtonState.Pressed)
            {
                Dragging = true;
                DragStart = Mouse.GetPosition(Canvas);
                SelectionRect.Visibility = System.Windows.Visibility.Visible;
            } 
            else if (Dragging && Mouse.LeftButton == MouseButtonState.Released)
            {
                if (Dragging)
                {
                    SelectEntities((Point)DragStart, Mouse.GetPosition(Canvas));
                }
                Dragging = false;
                DragStart = null;
                SelectionRect.Visibility = System.Windows.Visibility.Hidden;
            }

            if (Dragging)
            {
                var startX = DragStart.Value.X;
                var startY = DragStart.Value.Y;

                var cursorPos = Mouse.GetPosition(Canvas);
                var endX = cursorPos.X;
                var endY = cursorPos.Y;

                if (startX > endX)
                {
                    double tmp = startX;
                    startX = endX;
                    endX = tmp;
                }

                if (startY > endY)
                {
                    double tmp = startY;
                    startY = endY;
                    endY = tmp;
                }

                Canvas.SetLeft(SelectionRect, startX);
                Canvas.SetTop(SelectionRect, startY);
                SelectionRect.Width = endX - startX;
                SelectionRect.Height = endY - startY;
            }
        }

        private void SelectEntities(Point dragStart, Point dragEnd)
        {
            var startX = dragStart.X;
            var startY = dragStart.Y;

            var endX = dragEnd.X;
            var endY = dragEnd.Y;

            if (startX > endX)
            {
                double tmp = startX;
                startX = endX;
                endX = tmp;
            }

            if (startY > endY)
            {
                double tmp = startY;
                startY = endY;
                endY = tmp;
            }

            Rect selectionRect = new Rect((int) startX, (int) startY, (int) (endX - startX), (int) (endY - startY));

            foreach (var entity in EntityManager.GetEntities())
            {
                var selectionComp = entity.GetComponent<SelectionComponent>();
                var posComp = entity.GetComponent<PositionComponent>();
                var sizeComp = entity.GetComponent<SizeComponent>();
                if (selectionComp != null && posComp != null && sizeComp != null)
                {

                    Rect entityRect = new Rect(posComp.X, posComp.Y, sizeComp.Width, sizeComp.Height);
                    if (selectionRect.IntersectsWith(entityRect))
                    {
                        selectionComp.Selected = true;
                    }
                    else
                    {
                        selectionComp.Selected = false;
                    }
                }
            }
        }
    }
}
