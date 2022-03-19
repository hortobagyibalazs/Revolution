using System.Numerics;

namespace Revolution.IO
{
    public class MapData
    {
        public Vector2 Dimension { get; }
        
        /**
         * Stores entities based on X, Y coordinates
         */
        public int[,] Entities { get; }

        public MapData(Vector2 size)
        {
            Dimension = size;
            Entities = new int[(int) size.X, (int) size.Y];
        }
    }
}