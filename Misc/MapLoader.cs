using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Numerics;
using System.Xml;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.OpenGL.Imaging;
using Avalonia.Platform;
using Avalonia.Rendering;
using Avalonia.Visuals.Media.Imaging;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using TiledSharp;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using Image = Avalonia.Controls.Image;
using PixelFormat = Avalonia.Platform.PixelFormat;
using Size = Avalonia.Size;

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
            var bitmap = new Bitmap(tilesetPath);

            var mapData = new MapData(new Vector2(map.Width, map.Height));

            /*var mapBitmap = new Bitmap("Assets/test.png");
            var t = EntityManager.CreateEntity<Tile>();
            (t.GetComponent<RenderComponent>().Renderable as Image).Source = mapBitmap;
            t.GetComponent<GameMapObjectComponent>().Width = 100;
            t.GetComponent<GameMapObjectComponent>().Height = 100;
            return mapData;*/
            
            
            foreach (var layer in map.Layers)
            {
                int tilesInRow = 16;
                foreach (var tile in layer.Tiles)
                {
                    int gid = tile.Gid;
                    // Crop sprite from spritesheet
                    var croppedBitmap = new CroppedBitmap(bitmap,
                        new PixelRect((gid % tilesInRow - 1) * map.TileWidth, (gid / tilesInRow) * map.TileHeight,
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