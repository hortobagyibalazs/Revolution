using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Numerics;
using System.Xml;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using TiledSharp;

namespace Revolution.IO
{
    public class MapLoader
    {
        private static List<Entity> tiles = new List<Entity>();

        /**
         * @return Map dimension
         */
        public static Vector2 LoadFromFile(string tilesetPath, string tileMapPath)
        {
            var map = new TmxMap(tileMapPath);
            var bitmap = new Bitmap(tilesetPath);

            int maxWidth = Int32.MinValue;
            int maxHeight = Int32.MinValue;
            foreach (var layer in map.Layers)
            {
                int tilesInRow = 16;
                foreach (var tile in layer.Tiles)
                {
                    int gid = tile.Gid;
                    var croppedBitmap = new CroppedBitmap(bitmap,
                            new PixelRect((gid % tilesInRow - 1) * map.TileWidth, (gid / tilesInRow) * map.TileHeight,
                                map.TileWidth, map.TileHeight));

                    var tileEntity = EntityManager.CreateEntity<Tile>();
                    int tileWidth = tileEntity.GetComponent<SizeComponent>().Width;
                    int tileHeight = tileEntity.GetComponent<SizeComponent>().Height;
                    var xPos = tile.X * tileWidth;
                    var yPos = tile.Y * tileHeight;
                    
                    (tileEntity.GetComponent<RenderComponent>().Renderable as Image).Source = croppedBitmap;
                    tileEntity.GetComponent<PositionComponent>().X = xPos;
                    tileEntity.GetComponent<PositionComponent>().Y = yPos;

                    if (xPos + tileWidth > maxWidth)
                    {
                        maxWidth = xPos + tileWidth;
                    }

                    if (yPos + tileHeight > maxHeight)
                    {
                        maxHeight = yPos + tileHeight;
                    }
                }
            }

            return new Vector2(maxWidth, maxHeight);
        }

        public static void Unload()
        {
            foreach (var tile in tiles)
            {
                tile.Destroy();
            }

            tiles.Clear();
        }
    }
}