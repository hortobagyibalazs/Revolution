using Revolution.ECS.Components;
using Revolution.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Revolution.ECS.Entities
{
    public class Tree : Entity
    {
        public Tree()
        {
            var resourceComp = new ResourceComponent() { Wood = GlobalConfig.TreeResources };
            var posComp = new PositionComponent();
            var sizeComp = new SizeComponent();
            var mapObjectComp = new GameMapObjectComponent();
            var collisionComp = new CollisionComponent(mapObjectComp);

            mapObjectComp.PropertyChanged += delegate
            {
                sizeComp.Width = mapObjectComp.Width * GlobalConfig.TileSize;
                sizeComp.Height = mapObjectComp.Height * GlobalConfig.TileSize;
                posComp.X = mapObjectComp.X * GlobalConfig.TileSize;
                posComp.Y = mapObjectComp.Y * GlobalConfig.TileSize;
            };

            mapObjectComp.X = 1;
            mapObjectComp.Y = 1;
            mapObjectComp.Width = 1;
            mapObjectComp.Height = 1;

            AddComponent(posComp);
            AddComponent(sizeComp);
            AddComponent(mapObjectComp);
            AddComponent(resourceComp);
            AddComponent(collisionComp);
        }
    }
}
