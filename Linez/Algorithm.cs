using System;
using System.Collections.Generic;

namespace Linez
{
    public class Algorithm
    {
        public static bool Astar(List<List<int>> Maze, Coords start, Coords end)
        {
            Box startNode = new Box { position = start, parent = null, f = 0, g = 0, h = 0 };
            Box endNode = new Box { position = end, parent = null, f = 0, g = 0, h = 0 };
            List<Box> openList = new List<Box>();
            List<Box> closeList = new List<Box>();
            openList.Add(startNode);
            while (openList.Count > 0)
            {
                Console.WriteLine(openList.Count);
                Box currentNode = openList[0];
                int currentIndex = 0;
                for (var i = 0; i < openList.Count; i++)
                {
                    if (openList[i].f < currentNode.f)
                    {
                        currentIndex = i;
                        currentNode = openList[i];
                    }
                }
                openList.RemoveAt(currentIndex);
                closeList.Add(currentNode);
                Console.WriteLine(currentNode.position.x.ToString() + "," + currentNode.position.y.ToString());
                if (currentNode.position.x == endNode.position.x && currentNode.position.y == endNode.position.y)
                {
                    return true;
                }
                List<Box> children = new List<Box>();
                List<Coords> newPositions = new List<Coords>();
                newPositions.Add(new Coords { x = 0, y = 1 });
                newPositions.Add(new Coords { x = 1, y = 0 });
                newPositions.Add(new Coords { x = 0, y = -1 });
                newPositions.Add(new Coords { x = -1, y = 0 });
                for (var i = 0; i < newPositions.Count; i++)
                {
                    Coords nodePosition = new Coords { x = currentNode.position.x + newPositions[i].x, y = currentNode.position.y + newPositions[i].y };
                    if (nodePosition.x > 8 || nodePosition.x < 0 || nodePosition.y > 8 || nodePosition.y < 0)
                    {
                        continue;
                    }
                    if (Maze[nodePosition.x][nodePosition.y] == 1)
                    {
                        continue;
                    }
                    Box newNode = new Box { position = nodePosition, parent = currentNode.position };
                    children.Add(newNode);
                }
                for (var i = 0; i < children.Count; i++)
                {
                    bool checkCloseResult = true;
                    bool checkOpenResult = true;
                    foreach (var childClosed in closeList)
                    {
                        if (children[i].position.x == childClosed.position.x && children[i].position.y == childClosed.position.y)
                        {
                            checkCloseResult = false;
                            break;
                        }
                    }
                    if (checkCloseResult)
                    {
                        children[i].g = currentNode.g + 1;
                        children[i].h = ((children[i].position.x - endNode.position.x) * (children[i].position.x - endNode.position.x)) + ((children[i].position.y - endNode.position.y) * (children[i].position.y - endNode.position.y));
                        children[i].f = children[i].g + children[i].h;
                        foreach (var childOpen in openList)
                        {
                            if (children[i].position.x == childOpen.position.x && children[i].position.y == childOpen.position.y && children[i].g > childOpen.g)
                            {
                                checkOpenResult = false;
                                break;
                            }
                        }
                        if (checkOpenResult)
                        {
                            openList.Add(children[i]);
                        }
                    }
                }
            }
            return false;
        }

    }
}
