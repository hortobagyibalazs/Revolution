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

        public Map SelectedMap { get; set; }    

        public MapMenuManager()
        {
            this.AvailabeMaps = new List<Map>();
        }



        public IList<Map> LoadMaps()
        {
            ////var directoryPath = Path.Combine("Assets");
            ////var maps = Directory.GetFiles(directoryPath).Where(x => x.Contains(".tmx")).ToList();

            //return maps;
            return null;
        }

        public IList<Map> CreateMaps()
        {
            Map map1 = new Map()
            {
                Text = "Map 2",
                FilePath = @"D:\Egyetem\4.felev\SZTGUI\Féléves\Féléves\Revolution\Assets\map1.tmx",
                ImageSource = @"D:\Egyetem\4.felev\SZTGUI\Féléves\Féléves\Revolution\Assets\Images\MapSelectorMenuImages\map1.png"
            };

            Map map2 = new Map()
            {
                Text = "Map 1",
                FilePath = @"D:\Egyetem\4.felev\SZTGUI\Féléves\Féléves\Revolution\Assets\map2.tmx",
                ImageSource = @"D:\Egyetem\4.felev\SZTGUI\Féléves\Féléves\Revolution\Assets\Images\MapSelectorMenuImages\map2.png"
            };


            Map map3 = new Map()
            {
                Text = "Map 3",
                FilePath = @"D:\Egyetem\4.felev\SZTGUI\Féléves\Féléves\Revolution\Assets\map2.tmx",
                ImageSource = @"D:\Egyetem\4.felev\SZTGUI\Féléves\Féléves\Revolution\Assets\Images\MapSelectorMenuImages\map2.png"
            };

            AvailabeMaps.Add(map1);
            AvailabeMaps.Add(map2);
            AvailabeMaps.Add(map2);
            AvailabeMaps.Add(map2);
            AvailabeMaps.Add(map2);
            AvailabeMaps.Add(map2);
            AvailabeMaps.Add(map2);
            AvailabeMaps.Add(map2);

            AvailabeMaps.Add(map3);

            return AvailabeMaps;
        }
    }
}
