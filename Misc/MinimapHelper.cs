using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Revolution.Misc
{
    public class MinimapHelper
    {
        private Entity _entity;

        public MinimapHelper(Entity entity)
        {
            _entity = entity;
            _entity.DestroyEvent += _entity_DestroyEvent;
        }

        private void _entity_DestroyEvent(object? sender, Entity e)
        {
            _entity.DestroyEvent -= _entity_DestroyEvent;
            _entity = null;
        }

        public void DrawDynamicEntity(object? sender, MinimapDrawEventArgs e)
        {
            var mapObjectComp = _entity.GetComponent<GameMapObjectComponent>();
            var minimapComp = _entity.GetComponent<MinimapComponent>();

            if (mapObjectComp != null && minimapComp != null)
            {
                var dc = e.DrawingContext;
                var canvas = e.MinimapCanvas;

                double pxWidth = canvas.ActualWidth / e.GameMap.Dimension.X;
                double pxHeight = canvas.ActualHeight / e.GameMap.Dimension.Y;

                var rect = new Rect(mapObjectComp.X * pxWidth, mapObjectComp.Y * pxHeight, 
                    mapObjectComp.Width * pxWidth, mapObjectComp.Height * pxHeight);
                SolidColorBrush color = Brushes.Yellow;
                if (color == null) color = System.Windows.Media.Brushes.White;
                dc.DrawRectangle(color, null, rect);
            }
        }
    }
}
