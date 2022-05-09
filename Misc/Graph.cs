using Revolution.ECS.Entities;
using Revolution.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.Misc
{
    public class Graph
    {
        
        
        // Prints BFS traversal from a given source s
        // _V is number of vertices
        public static Dictionary<Vector2, Vector2> BFS(MapData _map, int _V, Vector2 start, Vector2 end)
        {

            // Mark all the vertices as not
            // visited(By default set as false)
            bool[] visited = new bool[(int) (_map.Dimension.X * _map.Dimension.Y)];
            for (int i = 0; i < _V; i++)
                visited[i] = !CellAvailable(_map, (int) ((i - start.Y) / _map.Dimension.Y), (int) ((i - start.X) / _map.Dimension.Y));

            // Create a queue for BFS
            LinkedList<Vector2> queue = new LinkedList<Vector2>();
            Dictionary<Vector2, Vector2> route = new Dictionary<Vector2, Vector2>(); // to, from

            // Mark the current node as
            // visited and enqueue it
            visited[(int) (start.Y * _map.Dimension.Y + start.X)] = true;
            queue.AddLast(start);

            while (queue.Any())
            {

                // Dequeue a vertex from queue
                // and print it
                start = queue.First();
                if (start == end) return route;
                queue.RemoveFirst();

                // Get all adjacent vertices of the
                // dequeued vertex s. If a adjacent
                // has not been visited, then mark it
                // visited and enqueue it
                List<Vector2> list = MapHelper.GetNeighbors(start, _map);

                foreach (var val in list)
                {
                    if (!visited[(int)(val.Y * _map.Dimension.Y + val.X)])
                    {
                        visited[(int)(val.Y * _map.Dimension.Y + val.X)] = true;
                        route[val] = start;
                        queue.AddLast(val);
                    }
                }
            }

            return route;
        }

        public static Queue<Vector2> PathFind(MapData map, int startX, int startY, int endX, int endY)
        {
            ExportCellAvailability(map);
            int vertices = (int) (map.Dimension.X * map.Dimension.Y);

            Vector2 end = new Vector2(endX, endY);
            Vector2 start = new Vector2(startX, startY);
            Dictionary<Vector2, Vector2> route = BFS(map, vertices, start, end);
            LinkedList<Vector2> TempPath = new LinkedList<Vector2>();

            Vector2? node = end;
            while (node != null)
            {
                TempPath.AddFirst((Vector2) node);
                if (route.ContainsKey((Vector2) node))
                {
                    node = route[(Vector2) node];
                } 
                else
                {
                    node = null;
                }
            }

            Queue<Vector2> Path = new Queue<Vector2>(TempPath.AsEnumerable());

            return Path;
        }

        private static bool CellAvailable(MapData map, int x, int y)
        {
            bool result = map.Entities[y, x] == null && map.Tiles[y, x].TrueForAll(tile => !tile.Colliding);
            return result;
        }

        public static void ExportCellAvailability(MapData map)
        {
            using (StreamWriter sw = new StreamWriter("test.txt"))
            {
                for (int i = 0; i < map.Dimension.X; i++)
                {
                    for (int j = 0; j < map.Dimension.Y; j++)
                    {
                        int value = 0; // empty
                        if (map.Tiles[j, i].Exists(tile => tile.Colliding) || map.Entities[j, i] != null)
                            value = 1;
                        sw.Write(value);
                    }
                    sw.WriteLine();
                }
            }
            ;
        }
    }
}
