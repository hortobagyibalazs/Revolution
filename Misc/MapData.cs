using Revolution.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Revolution.IO
{
    public class MapData
    {
        public Uri FileSource { get; set; }
        public List<Tile> Tiles { get; }
        public Vector2 Dimension { get; }
        public Entity[,] Entities { get; } // x,y

        public MapData(Vector2 size)
        {
            Dimension = size;
            Entities = new Entity[(int) Dimension.X, (int) Dimension.Y];
            Tiles = new List<Tile>();
        }
    }
}