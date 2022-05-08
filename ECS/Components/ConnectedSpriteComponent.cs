using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Revolution.ECS.Components
{
    public class ConnectedSpriteComponent : RenderComponent
    {
        public BitmapSource Tilesheet { get; set; } // the uncropped bitmap
        public string Id { get; set; }

        private string[] connectionBools = {
            "11111110",
            "10111111",
            "10111110",
            "11101111",
            "11101110",
            "10101111",
            "10101110",
            "11111011",
            "11111010",
            "10111011",
            "10111010",
            "11101011",
            "11101010",
            "10101011",
            "10101010",
            "11111000",
            "10111000",
            "11101000",
            "10101000",
            "00111110",
            "00101110",
            "00111010",
            "00101010",
            "10001111",
            "10001011",
            "10001110",
            "10001010",
            "11100011",
            "11100010",
            "10100011",
            "10100010",
            "10001000",
            "00100010",
            "00111000",
            "00101000",
            "00001110",
            "00001010",
            "10000011",
            "10000010",
            "11100000",
            "10100000",
            "00001000",
            "00100000",
            "10000000",
            "00000010",
            "00000000",
            "11111111"
        };

        public ConnectedSpriteComponent()
        {
            Renderable = new System.Windows.Controls.Image();
        }

        public void SetConnections(
            string connectionString)
        {
            var img = Renderable as System.Windows.Controls.Image;
            
            int index = connectionBools.ToList().IndexOf(connectionString);
            if (index != -1)
            {
                index += 2;
                var bitmap = GetBitmapBasedOnTileId(index);
                img.Source = bitmap;
            }
        }

        private CroppedBitmap GetBitmapBasedOnTileId(int actualGid)
        {
            int tilesInRow = 8;
            int tileWidth = 24;
            int tileHeight = 24;

            int startX = (actualGid % tilesInRow - 1) * tileWidth;
            int startY = (actualGid / tilesInRow) * tileHeight;
            if (actualGid % tilesInRow == 0)
            {
                startX = (tilesInRow - 1) * tileWidth;
                startY -= tileHeight;
            }

            // TODO : Clean up this part ASAP
            var cropRect = new Int32Rect(startX, startY, tileWidth, tileHeight);
            var croppedBitmap = new CroppedBitmap(Tilesheet, cropRect);
            croppedBitmap.Freeze();

            return croppedBitmap;
        }
    }
}
