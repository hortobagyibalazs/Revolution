using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.ECS.Components
{
	public class GridComponent : Component
	{
		private int width;
		private int height;
		private float cellSize;
		private int[,] gridArray;

		public int[,] GridArray
		{
			get { return gridArray; }
			set { gridArray = value; }
		}

		string gridFile = "grid.txt";

		public GridComponent(int width, int height, float cellSize)
		{
			this.width = width;
			this.height = height;
			this.cellSize = cellSize;
			this.gridArray = new int[width, height];

			string[]? lineData;
			int lineElement;

			using (StreamReader sr = new StreamReader(gridFile, Encoding.UTF8, true, width))
			{
				for (int i = 0; i < gridArray.GetLength(0); i++)
				{
					lineData = sr.ReadLine().Split(',');
					for (int j = 0; j < gridArray.GetLength(1); j++)
					{
						gridArray[i, j] = int.Parse(lineData[j]);
					}
				}
			}
		}

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