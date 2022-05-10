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

        public static int PeasantPriceWood = 0;
        public static int PeasantPriceGold = 50;
        public static int SwordsmanPriceWood = 0;
        public static int SwordsmanPriceGold = 150;
        public static int ArcherPriceWood = 50;
        public static int ArcherPriceGold = 110;

        public static int HousePriceWood = 100;
        public static int HousePriceGold = 50;
        public static int BarracksPriceWood = 350;
        public static int BarracksPriceGold = 50;
        public static int TownCenterPriceWood = 1000;
        public static int TownCenterPriceGold = 500;

    }

    public class GlobalStrings
    {
        public static readonly string NotEnoughResources = "Not enough resources";
        public static readonly string NotEnoughPopulationSpace = "Not enough population space";
        public static readonly string CheatActivated = "Cheat activated";
    }
}