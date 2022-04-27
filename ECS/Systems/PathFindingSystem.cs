using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.ECS.Systems
{
    public class PathFinderSystem : ISystem
    {
        GridComponent grid;

        public PathFinderSystem(GridComponent grid)
        {
            this.grid = grid;
        }

        public void UpdateCell(int x, int y, int data)
        {
            grid.GridArray[x, y] = data;
            grid.ExportGrid(grid.GridArray);
        }

        public void ExportGrid()
        {
            grid.ExportGrid(grid.GridArray);
        }

        public void Update(int deltaMs)
        {
            foreach (var entity in EntityManager.GetEntities())
            {
                if (entity is Entities.Villager)
                {
                    var movementComponent = entity.GetComponent<MovementComponent>();
                    var positionComponent = entity.GetComponent<PositionComponent>();
                    if (movementComponent != null && movementComponent.CurrentTarget != null)
                    {
                        movementComponent.Path.Clear();
                        grid.GridArray = null;
                        movementComponent.Path = PathFinding(positionComponent.Y / 64, positionComponent.X / 64, (int)movementComponent.CurrentTarget.Value.Y, (int)movementComponent.CurrentTarget.Value.X);
                        movementComponent.CurrentTarget = null;
                    }
                }
            }
        }

        public Queue<Vector2> PathFinding(int startX, int startY, int targetX, int targetY)
        {
            grid = new GridComponent(60, 60, 1);

            Queue<Vector2> Path = new Queue<Vector2>();

            int actualCell;
            int emptyCell = 0;
            int obstacleCell = -1;
            int startCell = -2;
            int targetCell = -3;

            grid.GridArray[startX, startY] = startCell;
            grid.GridArray[targetX, targetY] = targetCell;

            //for (int i = 0; i < grid.GridArray.GetLength(0); i++) //find startCell
            //{
            //	for (int j = 0; j < grid.GridArray.GetLength(1); j++)
            //	{
            //		if (grid.GridArray[i, j] == startCell)
            //		{
            //			startX = i; startY = j;
            //		}
            //	}
            //}

            //for (int i = 0; i < grid.GridArray.GetLength(0); i++) //find targetCell
            //{
            //	for (int j = 0; j < grid.GridArray.GetLength(1); j++)
            //	{
            //		if (grid.GridArray[i, j] == targetCell)
            //		{
            //			targetX = i; targetY = j;
            //		}
            //	}
            //}

            int localMin;
            int realMax = 9999;

            int[,] cloneGridArray = grid.GridArray;

            int newWaveFrontCell = 1;
            while (newWaveFrontCell > 0)
            {
                newWaveFrontCell = 0;

                for (int i = 1; i < grid.GridArray.GetLength(0) - 1; i++) //generate wavefront
                {
                    for (int j = 1; j < grid.GridArray.GetLength(1) - 1; j++)
                    {
                        if (grid.GridArray[i, j] == emptyCell)
                        {
                            localMin = realMax;
                            int minK = -1;
                            int minL = -1;

                            for (int k = -1; k < 2; k++)
                            {
                                for (int l = -1; l < 2; l++)
                                {
                                    actualCell = grid.GridArray[i + k, j + l];

                                    //Debug.WriteLine("Actual: " + (i + k) + " : " + (j + l));

                                    if (actualCell > emptyCell)
                                    {
                                        if (actualCell < localMin)
                                        {
                                            localMin = actualCell;
                                            minK = k;
                                            minL = l;
                                        }
                                    }
                                    else if (actualCell == targetCell)
                                    {
                                        localMin = emptyCell;
                                        minK = k;
                                        minL = l;
                                    }
                                    newWaveFrontCell++;
                                }
                            }
                            if (localMin < realMax)
                            {
                                if (Math.Abs(minK) == 1 && Math.Abs(minL) == 1)
                                {
                                    cloneGridArray[i, j] = localMin + 3;
                                }
                                else
                                {
                                    cloneGridArray[i, j] = localMin + 2;
                                }
                            }
                        }
                        else if (grid.GridArray[i, j] == startCell)
                        {
                            newWaveFrontCell = 0;
                        }
                    }
                }

                grid.GridArray = cloneGridArray;
            }

            ExportGrid();

            //check wavefront vs. startCell

            int localMax = -1;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (grid.GridArray[startX + i, startY + j] > 0)
                    {
                        localMax = grid.GridArray[startX + i, startY + j];
                    }
                }
            }

            bool pathAvailable;
            bool pathComplete = false;

            int travelerX = 0;
            int travelerY = 0;

            if (localMax > 0)
            {
                pathAvailable = true;
                //Debug.WriteLine("Path is available!");
            }
            else
            {
                pathAvailable = false;
                //Debug.WriteLine("Path is not available!");
            }

            int globalMax = -9999;

            if (pathAvailable)
            {
                for (int i = 0; i < grid.GridArray.GetLength(0); i++)
                {
                    for (int j = 0; j < grid.GridArray.GetLength(1); j++)
                    {
                        if (grid.GridArray[i, j] > globalMax)
                        {
                            globalMax = grid.GridArray[i, j];
                        }
                    }
                }
                if (localMax > 0)
                {
                    pathComplete = false;
                    travelerX = startX;
                    travelerY = startY;
                    //Debug.WriteLine("Traveler coordinates: " + travelerX + " : " + travelerY);
                }
            }

            localMin = 9999;

            int waveMinX = 9999;
            int waveMinY = 9999;

            while (!pathComplete)
            {


                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        actualCell = grid.GridArray[travelerX + i, travelerY + j];

                        if (actualCell == targetCell)
                        {
                            pathComplete = true;
                            waveMinX = travelerX + i;
                            waveMinY = travelerY + j;
                            break;
                        }
                        else if (actualCell > 0 && actualCell < localMin)
                        {
                            localMin = actualCell;
                            waveMinX = travelerX + i;
                            waveMinY = travelerY + j;
                        }
                    }
                }

                travelerX = waveMinX;
                travelerY = waveMinY;

                Path.Enqueue(new Vector2(travelerY, travelerX));
            }

            return Path;
        }
    }
}