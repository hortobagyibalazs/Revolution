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
            Panel.SetZIndex(SelectionRect, int.MaxValue);
            canvas.Children.Add(SelectionRect);
        }

        public void Update(int deltaMs)
        {
            if (!Dragging && Mouse.LeftButton == MouseButtonState.Pressed && Canvas.IsMouseOver)
            {
                Dragging = true;
                DragStart = Mouse.GetPosition(Canvas);
                SelectionRect.Visibility = System.Windows.Visibility.Visible;
            } 
            else if (Dragging && Mouse.LeftButton == MouseButtonState.Released && Canvas.IsMouseOver)
            {
                if (Dragging && Canvas.IsMouseOver)
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

            HashSet<Entity> selected = new HashSet<Entity>();
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
                        selected.Add(entity);
                    }
                }
            }

            // This part is to handle cases where some entities only allow single-selection
            // where others allow multi-selection
            foreach(var entity in EntityManager.GetEntities())
            {
                var selectionComp = entity.GetComponent<SelectionComponent>();
                if (selectionComp != null)
                {
                    if (selected.Contains(entity) && 
                        (selected.Count == 1 || selectionComp.MultiSelectable))
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
