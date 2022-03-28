using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using TiledSharp;
using Image = System.Windows.Controls.Image;

namespace Revolution.IO
{
    public class MapLoader
    {
        private static List<Entity> tiles = new List<Entity>();

        /**
         * @return Map dimension
         */
        public static MapData LoadFromFile(string tilesetPath, string tileMapPath)
        {
            var map = new TmxMap(tileMapPath);
            var bitmap = new BitmapImage(new Uri(tilesetPath, UriKind.Relative));

            var mapData = new MapData(new Vector2(map.Width, map.Height));
            
            foreach (var layer in map.Layers)
            {
                int tilesInRow = 16;
                foreach (var tile in layer.Tiles)
                {
                    int gid = tile.Gid;
                    // Crop sprite from spritesheet
                    var croppedBitmap = new CroppedBitmap(bitmap,
                        new Int32Rect((gid % tilesInRow - 1) * map.TileWidth, (gid / tilesInRow) * map.TileHeight,
                            map.TileWidth, map.TileHeight));
                    
                    var tileEntity = EntityManager.CreateEntity<Tile>();
                    var mapObjectComp = tileEntity.GetComponent<GameMapObjectComponent>();
                    
                    (tileEntity.GetComponent<RenderComponent>().Renderable as Image).Source = croppedBitmap;
                    mapObjectComp.X = tile.X;
                    mapObjectComp.Y = tile.Y;

                    mapData.Entities[tile.X, tile.Y] = tileEntity.Id;
                }
            }

            return mapData;
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