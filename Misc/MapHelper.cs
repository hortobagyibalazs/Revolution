using Revolution.ECS.Entities;
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
        public static Vector2? GetClosestEmptyCellToDesired(Vector2 spawnTarget, MapData gameMap, Entity target)
        {
            return GetClosestCellByPredicate(spawnTarget, gameMap, (vector) =>
            {
                var entity = gameMap.Entities[(int)vector.X, (int)vector.Y];
                return (entity is null || entity == target)
                && gameMap.Tiles[(int)vector.X, (int)vector.Y].TrueForAll(tile => !tile.Colliding);
            });
        }

        public static Vector2? GetClosestCellToEntityType(Type entityType, Vector2 target, MapData gameMap)
        {
            return GetClosestCellByPredicate(target, gameMap, (vector) => {
                var entity = gameMap.Entities[(int) vector.X, (int) vector.Y];
                return entity?.GetType() == entityType;
            });
        }

        public static Vector2? GetClosestCellByPredicate(Vector2 target, MapData gameMap, Func<Vector2, bool> predicate)
        {
            HashSet<Vector2> processed = new HashSet<Vector2>();
            Queue<Vector2> queue = new Queue<Vector2>();

            queue.Enqueue(target);
            processed.Add(target);
            while (queue.Count > 0)
            {
                var vector = queue.Dequeue();
                if (predicate(vector)) return vector;

                foreach (var neighbor in GetNeighbors(vector, gameMap))
                {
                    if (!processed.Contains(neighbor))
                    {
                        queue.Enqueue(neighbor);
                        processed.Add(neighbor);
                    }
                }
            }

            return null;
        }

        public static List<Vector2> GetNeighbors(Vector2 cell, MapData gameMap)
        {
            List<Vector2> neighbors = new List<Vector2>();

            int x = (int)cell.X;
            int y = (int)cell.Y;

            int minX = 0;
            int minY = 0;
            int maxX = (int) gameMap.Dimension.X - 1;
            int maxY = (int) gameMap.Dimension.Y - 1;

            for (int _x = x - 1; _x <= x + 1; _x++)
            {
                for (int _y = y - 1; _y <= y + 1; _y++)
                {
                    if (_x >= minX && _x <= maxX && _y >= minY && _y <= maxY 
                        && (_x == x || _x == x + 1 || _y == y + 1 || _y == y))
                    {
                        neighbors.Add(new Vector2(_x, _y));
                    }
                }
            }

            return neighbors;
        }
    }
}
