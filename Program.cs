using System;
using System.Collections.Generic;

namespace AdvCode2025_4
{
    class Program
    {
        static public List<List<bool>> Parse(string input)
        {
            string[] lines = input.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            List<List<bool>> output = new List<List<bool>>();
            int maxLength = 0;
            
            foreach (string line in lines)
            {
                if (line.Length > maxLength)
                {
                    maxLength = line.Length;
                }
            }
            
            foreach (string line in lines)
            {
                List<bool> boolList = new List<bool>();
                foreach (char c in line)
                {
                    boolList.Add(c == '@');
                }
                
                while (boolList.Count < maxLength)
                {
                    boolList.Add(false);
                }

                output.Add(boolList);
            }

            return output;
        }

        static public int AccessibleForklifts(List<List<bool>> grid, List<(int, int)> toRemove)
        {
            int rowCount = grid.Count;
            int colCount = grid[0].Count;
            int accessibleCount = 0;
            
            int[][] directions = new int[][]
            {
                new[] { -1, -1 }, new[] { -1, 0 }, new[] { -1, 1 },
                new[] { 0, -1 },                new[] { 0, 1 },
                new[] { 1, -1 }, new[] { 1, 0 }, new[] { 1, 1 }
            };

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    if (grid[i][j])
                    {
                        int adjacentCount = 0;
                        
                        foreach (var dir in directions)
                        {
                            int newRow = i + dir[0];
                            int newCol = j + dir[1];
                            
                            if (newRow >= 0 && newRow < rowCount && newCol >= 0 && newCol < colCount)
                            {
                                if (grid[newRow][newCol]) adjacentCount++;
                            }
                        }
                        
                        if (adjacentCount < 4)
                        {
                            toRemove.Add((i, j));
                            accessibleCount++;
                        }
                    }
                }
            }

            return accessibleCount;
        }

        static public int RemoveAccessibleRolls(List<List<bool>> grid)
        {
            int totalRemoved = 0;
            bool hasRemoved = true;

            while (hasRemoved)
            {
                hasRemoved = false;
                List<(int, int)> toRemove = new List<(int, int)>();
                
                int accessibleCount = AccessibleForklifts(grid, toRemove);
                totalRemoved += accessibleCount;
                
                foreach (var pos in toRemove)
                {
                    grid[pos.Item1][pos.Item2] = false;
                }

                if (accessibleCount > 0) hasRemoved = true;
            }

            return totalRemoved;
        }

        static void Main(string[] args)
        {
            string input = System.IO.File.ReadAllText("input.txt");
            List<List<bool>> grid = Parse(input);

            int totalRollsRemoved = RemoveAccessibleRolls(grid);
            Console.WriteLine($"Total rolls of paper removed: {totalRollsRemoved}");
        }
    }
}
