using Revolution.ECS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.Misc
{
    public class MapMenuManager
    {
        public IList<Map> AvailabeMaps { get; set; }

        

        public MapMenuManager()
        {
            this.AvailabeMaps = new List<Map>();
        }

        //public IList<Map> LoadMaps()
        //{
        //    ////var directoryPath = Path.Combine("Assets");
        //    ////var maps = Directory.GetFiles(directoryPath).Where(x => x.Contains(".tmx")).ToList();

        //    //return maps;
        //    return null;
        //}

        public IList<Map> CreateMaps()
        {
            Map map1 = new Map()
            {
                Text = "Coastal",
                FilePath = @"Assets\map1.tmx",
                ImageSource = "\\Assets\\Images\\MapSelectorMenuImages\\map1.png"
            };

            Map map2 = new Map()
            {
                Text = "Shite",
                FilePath = @"Assets\map2.tmx",
                ImageSource = "\\Assets\\Images\\MapSelectorMenuImages\\map2.png"
            };
            
            AvailabeMaps.Add(map1);
            AvailabeMaps.Add(map2);

            return AvailabeMaps;
        }

        private Map selectedMap;

        public Map SelectedMap
        {
            get { return selectedMap; }
            set { selectedMap = value; }
        }

    }
}
