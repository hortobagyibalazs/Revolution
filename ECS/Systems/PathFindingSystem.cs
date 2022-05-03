using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using Revolution.Commands;
using Revolution.ECS.Components;
using Revolution.ECS.Entities;
using Revolution.IO;
using Revolution.Misc;
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
    public class PathFinderSystem : ISystem, IRecipient<FindRouteCommand>, IRecipient<FindRouteToEntityTypeCommand>
    {
        private IMessenger _messenger = Ioc.Default.GetService<IMessenger>();
        private Queue<FindRouteCommand> _commandQueue;

        GridComponent grid;
        MapData mapData;

        public PathFinderSystem(MapData mapData)
        {
            this.mapData = mapData;

            _commandQueue = new Queue<FindRouteCommand>();
            _messenger.Register<FindRouteCommand>(this);
            _messenger.Register<FindRouteToEntityTypeCommand>(this);
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

        public void Receive(FindRouteCommand message)
        {
            var entity = message.Entity;
            var dest = message.Destination;
            var movementComponent = entity.GetComponent<MovementComponent>();
            var gameMapObjectComponent = entity.GetComponent<GameMapObjectComponent>();
            if (movementComponent != null)
            {
                movementComponent.Path.Clear();
                if (grid != null)
                {
                    grid.GridArray = null;
                }
                var closestCell = MapHelper.GetClosestEmptyCellToDesired(dest, mapData);
                movementComponent.Path = PathFinding(mapData, gameMapObjectComponent.Y, gameMapObjectComponent.X, (int)((Vector2)closestCell).Y, (int)((Vector2)closestCell).X);
            }
        }

        public void Receive(FindRouteToEntityTypeCommand message)
        {
            var mapObjectComp = message.Entity.GetComponent<GameMapObjectComponent>();
            var currentPos = new Vector2(mapObjectComp.X, mapObjectComp.Y);
            var cell = MapHelper.GetClosestCellToEntityType(message.DestEntityType, currentPos, mapData);
            if (cell != null)
            {
                var command = new FindRouteCommand(message.Entity, (Vector2)cell);
                Receive(command);
            }
        }

        public void Update(int deltaMs)
        {
            FindRouteCommand result = null;
            bool success = _commandQueue.TryDequeue(out result);
            if (success)
            {
                Receive(result);
            }
        }

        public Queue<Vector2> PathFinding(MapData mapData, int startX, int startY, int targetX, int targetY)
        {
            grid = new GridComponent(mapData);

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