using System;
using System.Collections.Generic;

namespace Linez
{
    public class Processing
    {
        public static Coords RandomPlaceBall(List<List<int>> Maze, List<List<string>> ColorMaze, string Color)
        {
            Random rand = new Random();
            int row = rand.Next(9);
            int col = rand.Next(9);
            if (Maze[col][row] == 0)
            {
                Maze[col][row] = 1;
                ColorMaze[col][row] = Color;
                return new Coords
                {
                    x = col,
                    y = row
                };
            }
            else
            {
                return RandomPlaceBall(Maze, ColorMaze, Color);
            }
        }
        public static bool CheckRoom(List<List<int>> Maze)
        {
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    if (Maze[i][j] == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static string GetRandomColor(List<string> Colors)
        {
            Random rand = new Random();
            return Colors[rand.Next(7)];
        }
        public static void UpdateMazeToOne(List<List<int>> Maze, int x, int y)
        {
            Maze[x][y] = 1;
        }
        public static void UpdateMazeToZero(List<List<int>> Maze, int x, int y)
        {
            Maze[x][y] = 0;
        }
        public static void UpdateColorMazeToColor(List<List<string>> Maze, int x, int y, string color)
        {
            Maze[x][y] = color;
        }
        public static void UpdateColorMazeToNull(List<List<string>> Maze, int x, int y)
        {
            Maze[x][y] = null;
        }
        
    }
}
