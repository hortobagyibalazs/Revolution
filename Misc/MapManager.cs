namespace Revolution.IO
{
    public class MapManager
    {
        public static readonly MapManager Instance = new MapManager();
        public MapData Map { get; }
        
        private MapManager()
        {
            
        }
    }
}