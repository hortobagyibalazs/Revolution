using Revolution.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Revolution.IO
{
    public class MapData
    {
        public Uri FileSource { get; set; }
        public Vector2 Dimension { get; } // width, height (in terms of number of cells)
        public Entity[,] Entities { get; } // x,y
        public List<Tile>[,] Tiles { get; } // matrix of list tiles

        public MapData(Vector2 size)
        {
            Dimension = size;
            Entities = new Entity[(int) Dimension.X, (int) Dimension.Y];

            Tiles = new List<Tile>[(int) Dimension.X, (int) Dimension.Y];
            for (int i = 0; i < Dimension.X; i++)
            {
                for (int j = 0; j < Dimension.Y; j++)
                {
                    Tiles[i, j] = new List<Tile>();
                }
            }
        }
    }
}