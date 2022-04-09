using Revolution.ECS.Components;
using Revolution.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Revolution.ECS.Entities
{
    public class Goldmine : Entity
    {
        public Goldmine()
        {
            var resourceComp = new ResourceComponent() { Gold = GlobalConfig.GOLD_MINE_SIZE };
            var renderComp = new SpriteComponent() { Source = new Uri(@"\Assets\Images\spr_gold_mine.png", UriKind.Relative) };
            var posComp = new PositionComponent();
            var sizeComp = new SizeComponent();
            var mapObjectComp = new GameMapObjectComponent();
            var collisionComp = new CollisionComponent(mapObjectComp);
            var minimapComp = new MinimapComponent() { Background = Brushes.Gray };

            sizeComp.PropertyChanged += delegate
            {
                (renderComp.Renderable).Width = sizeComp.Width;
                (renderComp.Renderable).Height = sizeComp.Height;
            };

            posComp.PropertyChanged += delegate
            {
                Canvas.SetLeft(renderComp.Renderable, posComp.X);
                Canvas.SetTop(renderComp.Renderable, posComp.Y);
            };

            mapObjectComp.PropertyChanged += delegate
            {
                sizeComp.Width = mapObjectComp.Width * GlobalConfig.TileSize;
                sizeComp.Height = mapObjectComp.Height * GlobalConfig.TileSize;
                posComp.X = mapObjectComp.X * GlobalConfig.TileSize;
                posComp.Y = mapObjectComp.Y * GlobalConfig.TileSize;
            };

            mapObjectComp.X = 1;
            mapObjectComp.Y = 1;
            mapObjectComp.Width = 3;
            mapObjectComp.Height = 3;

            AddComponent(renderComp);
            AddComponent(posComp);
            AddComponent(sizeComp);
            AddComponent(mapObjectComp);
            AddComponent(resourceComp);
            AddComponent(collisionComp);
            AddComponent(minimapComp);
        }
    }
}
