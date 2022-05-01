using Revolution.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.Misc
{
    public class MapHelper
    {
        public static Vector2? GetClosestEmptyCellToDesired(Vector2 spawnTarget, MapData gameMap)
        {
            int targetX = (int)spawnTarget.X;
            int targetY = (int)spawnTarget.Y;

            // Look for closest empty cell in spiral
            int maxDistance = (int) gameMap.Dimension.X / 2;

            for (int distance = 0; distance < maxDistance; distance++)
            {
                int startX = targetX - distance;
                int endX = targetX + distance;
                int startY = targetY - distance;
                int endY = targetY + distance;

                for (int x = startX; x <= endX; x++)
                {
                    for (int y = endY; y >= startX; y--)
                    {
                        if ((x == startX || y == startY || x == endX || y == endY)
                           && startX >= 0 && startY >= 0 && endX < gameMap.Dimension.X && endY < gameMap.Dimension.Y)
                        {
                            if (gameMap.Entities[x, y] == null)
                            {
                                return new Vector2(x, y);
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}
