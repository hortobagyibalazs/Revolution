namespace Revolution.IO
{
    public class GlobalConfig
    {
        public static int TileSize = 64;
        public static int GoldMineResources = 5000;
        public static int TreeResources = 50;
        public static int StarterGold = 200;
        public static int StarterWood = 200;
        public static int PeasantWoodCuttingRate = 21; // the higher the slower
        public static int PeasantGoldMiningRate = 21; // the higher the slower
        public static int PeasantWoodCapacity = 10;
        public static int PeasantGoldCapacity = 10;
        public static int HouseBuildPoints = 500;
        public static int BarracksBuildPoints = 1100;
    }

    public class GlobalStrings
    {
        public static readonly string NotEnoughResources = "Not enough resources";
    }
}