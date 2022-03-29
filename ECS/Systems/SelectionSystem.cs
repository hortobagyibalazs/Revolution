using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Revolution.ECS.Systems
{
    public class SelectionSystem : ISystem
    {
        private Canvas Canvas;
        private ScrollViewer ScrollViewer;

        public SelectionSystem(Canvas canvas, ScrollViewer scrollViewer)
        {
            Canvas = canvas;
            ScrollViewer = scrollViewer;
        }

        public void Update(int deltaMs)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                var mousePos = Mouse.GetPosition(ScrollViewer);
                var cursorX = (int)(mousePos.X + ScrollViewer.HorizontalOffset);
                var cursorY = (int)(mousePos.Y + ScrollViewer.VerticalOffset);

                // TODO : Make it so that only one entity can be selected at a time
                foreach (var entity in EntityManager.GetEntities())
                {
                    var selectionComp = entity.GetComponent<SelectionComponent>();
                    var posComp = entity.GetComponent<PositionComponent>();
                    var sizeComp = entity.GetComponent<SizeComponent>();
                    if (selectionComp != null && posComp != null && sizeComp != null)
                    {
                        if (posComp.X <= cursorX && posComp.X + sizeComp.Width >= cursorX 
                            && posComp.Y <= cursorY && posComp.Y + sizeComp.Height >= cursorY)
                        {
                            // Clicked on entity
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
}
