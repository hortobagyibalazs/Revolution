namespace Revolution.IO
{
    public class MapManager
    {
        public readonly MapManager Instance = new MapManager();
        public MapData Map { get; }
        
        private MapManager()
        {
            
        }
    }
}