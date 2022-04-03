using Revolution.ECS.Entities;
using System.Collections.Generic;
using System.Numerics;

namespace Revolution.IO
{
    public class MapData
    {
        public Vector2 Dimension { get; }
        
        /**
         * Stores entities based on X, Y coordinates
         */
        public LinkedList<Entity>[,] Entities { get; }

        public MapData(Vector2 size)
        {
            Dimension = size;
            Entities = new LinkedList<Entity>[(int) size.X, (int) size.Y];
        }
    }
}