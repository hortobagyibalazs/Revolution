using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
        /**
         * @return Map dimension
         */
        public static MapData LoadFromFile(string tilesetPath, string tileMapPath)
        {
            var map = new TmxMap(tileMapPath);

            IDictionary<TmxTileset, BitmapImage> bitmaps = new Dictionary<TmxTileset, BitmapImage>();
            foreach (var tileset in map.Tilesets)
            {
                bitmaps[tileset] = new BitmapImage(new Uri(@tilesetPath + tileset.Image.Source, UriKind.Relative));
            }

            var mapData = new MapData(new Vector2(map.Width, map.Height));
            mapData.FileSource = new Uri(tileMapPath, UriKind.Relative);
            int tilesInRow = 8;

            int zIndex = -1;
            foreach (var layer in map.Layers)
            {
                zIndex++;
                foreach (var tile in layer.Tiles)
                {
                    
                    int gid = tile.Gid;
                    // Find tileset for tile
                    var tileset = GetTilesetForGid(map.Tilesets, tile.Gid);
                    
                    if (tileset == null) continue;

                    try
                    {
                        var entity = CreateEntity(tileset, gid, mapData, tilesInRow, map, tile, bitmaps[tileset]);
                        if (entity == null)
                        {
                            continue;
                        }
                        var mapObjectComp = entity.GetComponent<GameMapObjectComponent>();

                        var renderComp = entity.GetComponent<RenderComponent>();
                        if (renderComp != null)
                        {
                            renderComp.ZIndex = zIndex;
                        }
                        mapObjectComp.X = tile.X;
                        mapObjectComp.Y = tile.Y - mapObjectComp.Height + 1;
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

        private static int townCenter = 0;
        private static Entity CreateEntity(TmxTileset tileset, int gid, MapData mapData, int tilesInRow,
            TmxMap map, TmxLayerTile tile, BitmapSource bitmap)
        {
            int actualGid = gid - tileset.FirstGid + 1;

            Entity entity = null;
            if (tileset.Name == "gold_mine")
            {
                entity = EntityManager.CreateEntity<Goldmine>();
            }
            else if (tileset.Name == "town_center")
            {
                entity = EntityManager.CreateEntity<TownCenter>();
                entity.GetComponent<BuildingComponent>().State = BuildingState.Built;
                entity.GetComponent<TeamComponent>().TeamId = townCenter++;
            }
            else if (tileset.Name == "bgd_trees")
            {
                if (actualGid != 48)
                {
                    entity = EntityManager.CreateEntity<Tree>();
                }
                CreateTile(mapData, actualGid, tilesInRow, tileset, map, tile, bitmap, true, typeof(Tree));
            }
            else if (tileset.Name == "vertical_brige")
            {
                CreateTile(mapData, actualGid, tilesInRow, tileset, map, tile, bitmap);
            }
            else
            {
                //entity = EntityManager.CreateEntity<Tile>();
                CreateTile(mapData, actualGid, tilesInRow, tileset, map, tile, bitmap);
            }

            return entity;
        }

        private static void CreateTile(MapData mapData, int actualGid, int tilesInRow, 
            TmxTileset tileset, TmxMap map, TmxLayerTile tile, BitmapSource bitmap, bool colliding = false, Type? entityType = null)
        {
            int startX = (actualGid % tilesInRow - 1) * map.TileWidth;
            int startY = (actualGid / tilesInRow) * map.TileHeight;
            if (actualGid % tilesInRow == 0)
            {
                startX = (tilesInRow - 1) * map.TileWidth;
                startY -= map.TileHeight;
            }

            // TODO : Clean up this part ASAP
            var cropRect = new Int32Rect(startX, startY, tileset.TileWidth, tileset.TileHeight);
            var croppedBitmap = new CroppedBitmap(bitmap, cropRect);
            croppedBitmap.Freeze();

            var tileObj = new Tile()
            {
                Drawable = croppedBitmap,
                CellX = tile.X,
                CellY = tile.Y,
                Width = tileset.TileWidth,
                Height = tileset.TileHeight,
                Colliding = colliding,
                EntityType = entityType,
                Color = GetMinimapColorForTile(tileset)
            };

            mapData.Tiles[tile.X, tile.Y].Add(tileObj);
        }

        private static SolidColorBrush GetMinimapColorForTile(TmxTileset tileset)
        {
            var brush = System.Windows.Media.Brushes.Yellow;
            if (tileset.Name == "bgd_dirt")
            {
                brush = System.Windows.Media.Brushes.Brown;
            }
            else if (tileset.Name == "bgd_water")
            {
                brush = System.Windows.Media.Brushes.MediumBlue;
            }
            else if (tileset.Name == "bgd_grass")
            {
                brush = System.Windows.Media.Brushes.ForestGreen;
            }
            else if (tileset.Name == "bgd_trees")
            {
                brush = System.Windows.Media.Brushes.DarkGreen;
            }
            else if (tileset.Name == "gold_mine")
            {
                brush = System.Windows.Media.Brushes.Gray;
            }

            return brush;
        }
    }
}