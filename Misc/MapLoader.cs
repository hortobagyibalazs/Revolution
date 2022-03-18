using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        public static void LoadFromFile(string tilesetPath, string tileMapPath)
        {
            var map = new TmxMap(tileMapPath);
            var bitmap = new Bitmap(tilesetPath);

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

                    (tileEntity.GetComponent<RenderComponent>().Renderable as Image).Source = croppedBitmap;
                    tileEntity.GetComponent<PositionComponent>().X = tile.X * tileWidth;
                    tileEntity.GetComponent<PositionComponent>().Y = tile.Y * tileHeight;

                }
            }
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