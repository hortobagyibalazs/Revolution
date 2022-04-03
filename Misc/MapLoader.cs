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

            //GlobalConfig.TileSize = map.TileWidth;

            IDictionary<TmxTileset, BitmapImage> bitmaps = new Dictionary<TmxTileset, BitmapImage>();
            foreach (var tileset in map.Tilesets)
            {
                bitmaps[tileset] = new BitmapImage(new Uri(@tilesetPath + tileset.Image.Source, UriKind.Relative));
            }

            var mapData = new MapData(new Vector2(map.Width, map.Height));
            int tilesInRow = 8;

            foreach (var layer in map.Layers)
            {
                foreach (var tile in layer.Tiles)
                {
                    
                    int gid = tile.Gid;
                    // Find tileset for tile
                    var tileset = GetTilesetForGid(map.Tilesets, tile.Gid);
                    if (tileset == null) continue;
                    int actualGid = gid - tileset.FirstGid + 1;
                    Debug.WriteLine($"{actualGid} GID from {tileset.Name}");

                    try
                    {
                        int startX = (actualGid % tilesInRow - 1) * map.TileWidth;
                        int startY = (actualGid / tilesInRow) * map.TileHeight;
                        if (actualGid % tilesInRow == 0)
                        {
                            startX = (tilesInRow - 1) * map.TileWidth;
                            startY -= map.TileHeight;
                            ;
                        }

                        var cropRect = new Int32Rect(startX, startY, map.TileWidth, map.TileHeight);
                        var croppedBitmap = new CroppedBitmap(bitmaps[tileset], cropRect);

                        var tileEntity = EntityManager.CreateEntity<Tile>();
                        var mapObjectComp = tileEntity.GetComponent<GameMapObjectComponent>();

                        (tileEntity.GetComponent<RenderComponent>().Renderable as Image).Source = croppedBitmap;
                        mapObjectComp.X = tile.X;
                        mapObjectComp.Y = tile.Y;

                        mapData.Entities[tile.X, tile.Y] = tileEntity.Id;

                    } 
                    catch 
                    {
                         
                    }
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

        private static TmxTileset GetTilesetForGid(TmxList<TmxTileset> tilesets, int gid)
        {
            int lastFirstGid = -1;
            TmxTileset lastTileset = null;
            foreach(var tileset in tilesets)
            {
                if (tileset.FirstGid == gid)
                {
                    return tileset;
                }
                else if (tileset.FirstGid > gid)
                {
                    return lastTileset;
                }
                else
                {
                    lastFirstGid = tileset.FirstGid;
                    lastTileset = tileset;
                }
            }

            return null;
        }
    }
}