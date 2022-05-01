using Revolution.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.ECS.Components
{
	public class GridComponent : Component
	{
		//private int width;
		public int width;
		//private int height;
		public int height;
		private float cellSize;
		private int[,] gridArray;

		public int[,] GridArray
		{
			get { return gridArray; }
			set { gridArray = value; }
		}

		string gridFile = "grid.txt";

        public GridComponent(MapData mapData)
        {
			height = mapData.Entities.GetLength(0);
			width = mapData.Entities.GetLength(1);
			gridArray = new int[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
					gridArray[i, j] = -1;
                }
			}

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (mapData.Entities[i,j] == null)
                    {
						gridArray[i,j] = 0;
                    }
                }
            }
        }

		//public GridComponent(int width, int height, float cellSize)
		//{
		//	this.width = width;
		//	this.height = height;
		//	this.cellSize = cellSize;
		//	this.gridArray = new int[width, height];

		//	string[]? lineData;
		//	int lineElement;

		//	using (StreamReader sr = new StreamReader(gridFile, Encoding.UTF8, true, width))
		//	{
		//		for (int i = 0; i < gridArray.GetLength(0); i++)
		//		{
		//			lineData = sr.ReadLine().Split(',');
		//			for (int j = 0; j < gridArray.GetLength(1); j++)
		//			{
		//				gridArray[i, j] = int.Parse(lineData[j]);
		//			}
		//		}
		//	}
		//}

		public void ExportGrid(int[,] gridArray)
		{
			using (StreamWriter sw = new StreamWriter("grid_output.txt"))
			{
				for (int i = 0; i < gridArray.GetLength(0); i++)
				{
					for (int j = 0; j < gridArray.GetLength(1); j++)
					{
						sw.Write(gridArray[i, j]);
						sw.Write(',');
					}
					sw.WriteLine();
				}
			}
		}
	}
}