using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AOC {
    // Naive algorithm
    class Day11 {
        private static Grid ParseInput() {
            Grid grid = new Grid(97, 98);
            int y = 0;
            using (StreamReader reader = new StreamReader("Inputs/day11.txt")) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    for (int i = 0; i < line.Length; i++)
                        grid.Insert(y, i, line.Substring(i,1));
                    y++;
                }
            }
            return grid;
        }

        public static void SolveDay11_1() {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Grid grid = ParseInput();
            // Any value that satisfies the condition works 
            int changed, ocuppiedSeats;
            changed = ocuppiedSeats = 1;
            while (changed > 0) {
                Grid newArrangement = new Grid(grid.SizeY, grid.SizeX);
                changed = ocuppiedSeats = 0;
                for (int y = 0; y < grid.SizeY; y++)
                    for (int x = 0; x < grid.SizeX; x++) {
                        newArrangement.Insert(y, x, grid.GetNewPart1RulesValue(y, x));
                        if (!newArrangement.GetValue(y, x).Equals(grid.GetValue(y, x))) changed++;
                        if (newArrangement.GetValue(y, x).Equals(Grid.OCCUPIED_SEAT_TOKEN)) ocuppiedSeats++;
                    }
                grid = newArrangement;
            }
            sw.Stop();
            Console.WriteLine($"There are {ocuppiedSeats} occupied seats.");
            Console.WriteLine($"The program finished in {sw.ElapsedMilliseconds}ms");
        }

        public static void SolveDay11_2() {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Grid grid = ParseInput();
            // Any value that satisfies the condition works 
            int changed, ocuppiedSeats;
            changed = ocuppiedSeats = 1;
            while (changed > 0) {
                Grid newArrangement = new Grid(grid.SizeY, grid.SizeX);
                changed = ocuppiedSeats = 0;
                for (int y = 0; y < grid.SizeY; y++)
                    for (int x = 0; x < grid.SizeX; x++) {
                        newArrangement.Insert(y, x, grid.GetNewPart2RulesValue(y, x));
                        if (!newArrangement.GetValue(y, x).Equals(grid.GetValue(y, x))) changed++;
                        if (newArrangement.GetValue(y, x).Equals(Grid.OCCUPIED_SEAT_TOKEN)) ocuppiedSeats++;
                    }
                grid = newArrangement;
            }
            sw.Stop();
            Console.WriteLine($"There are {ocuppiedSeats} occupied seats.");
            Console.WriteLine($"The program finished in {sw.ElapsedMilliseconds}ms");
        }

        class Grid {
            string[][] graph;
            private int sizeX;
            private int sizeY;
            public const string SEAT_TOKEN = "L";
            public const string OCCUPIED_SEAT_TOKEN = "#";
            public const string FLOOR_TOKEN = ".";
            private readonly HashSet<string> VALID_VALUES = new HashSet<string>() { SEAT_TOKEN, OCCUPIED_SEAT_TOKEN, FLOOR_TOKEN };

            public int SizeX { get => sizeX; set => sizeX = value; }
            public int SizeY { get => sizeY; set => sizeY = value; }

            public Grid(int dimY, int dimX) {
                graph = new string[dimY][];
                for (int i = 0; i < dimY; i++) {
                    graph[i] = new string[dimX];
                }
                sizeX = dimX;
                sizeY = dimY;
            }

            public void Insert(int y, int x, string value) {
                if (!VALID_VALUES.Contains(value)) return;
                graph[y][x] = value;
            }

            public string GetValue(int y, int x) {
                return graph[y][x];
            }

            public string GetNewPart1RulesValue(int y, int x) {
                switch (graph[y][x]) {
                    case SEAT_TOKEN:
                        return GetNewPart1RulesSeatValue(y, x);
                    case OCCUPIED_SEAT_TOKEN:
                        return GetNewPart1RulesOccupiedSeatValue(y, x);
                    default:
                        return graph[y][x];
                }
            }

            private string GetNewPart1RulesSeatValue(int y, int x) {
                for (int i = 1; i >= -1; i--)
                    for (int j = 1; j >= -1; j--) {
                        // It is the value we are cheching
                        if (i == j && j == 0) continue;
                        // If any of the values is out of bounds we skip
                        // It would be better to check y in the outter loop but I'm lazy
                        else if (y + i >= SizeY 
                            || x + j >= sizeX
                            || y + i < 0 
                            || x + j < 0) continue;
                        else if (graph[y + i][x + j].Equals(OCCUPIED_SEAT_TOKEN)) return SEAT_TOKEN;
                    }
                return OCCUPIED_SEAT_TOKEN;
            }

            private string GetNewPart1RulesOccupiedSeatValue(int y, int x) {
                int occupied = 0;
                for (int i = 1; i >= -1; i--)
                    for (int j = 1; j >= -1; j--) {
                        // It is the value we are cheching
                        if (i == j && j == 0) continue;
                        // If any of the values is out of bounds we skip
                        // It would be better to check y in the outter loop but I'm lazy
                        // Again we should stop after occupied is greater or equal to 4 but I'm lazy
                        else if (y + i >= SizeY 
                            || x + j >= sizeX
                            || y + i < 0 
                            || x + j < 0) continue;
                        else if (graph[y + i][x + j].Equals(OCCUPIED_SEAT_TOKEN)) occupied++;
                    }
                if (occupied >= 4) return SEAT_TOKEN;
                return OCCUPIED_SEAT_TOKEN;
            }

            public string GetNewPart2RulesValue(int y, int x) {
                switch (graph[y][x]) {
                    case SEAT_TOKEN:
                        return GetNewPart2RulesSeatValue(y, x);
                    case OCCUPIED_SEAT_TOKEN:
                        return GetNewPart2RulesOccupiedSeatValue(y, x);
                    default:
                        return graph[y][x];
                }
            }

            private string GetNewPart2RulesSeatValue(int y, int x) {
                for (int i = 1; i >= -1; i--)
                    for (int j = 1; j >= -1; j--) {
                        // It is the value we are cheching
                        if (i == j && j == 0) continue;
                        else if (IsLineOccupied(y, x, i, j)) return SEAT_TOKEN;
                    }
                return OCCUPIED_SEAT_TOKEN;
            }

            private string GetNewPart2RulesOccupiedSeatValue(int y, int x) {
                int occupied = 0;
                for (int i = 1; i >= -1; i--)
                    for (int j = 1; j >= -1; j--) {
                        // It is the value we are cheching
                        if (i == j && j == 0) continue;
                        else if (IsLineOccupied(y, x, i, j)) occupied++;
                    }
                if (occupied >= 5) return SEAT_TOKEN;
                return OCCUPIED_SEAT_TOKEN;
            }

            private bool IsLineOccupied(int y, int x, int yIncrement, int xIncrement) {
                int curX = x + xIncrement;
                int curY = y + yIncrement;
                while (curX >= 0 && curY >= 0 && curX < SizeX && curY < sizeY) {
                    switch (graph[curY][curX]) {
                        case OCCUPIED_SEAT_TOKEN:
                            return true;
                        case SEAT_TOKEN:
                            return false;
                        default:
                            curX += xIncrement;
                            curY += yIncrement;
                            break;
                    }
                }
                return false;
            }


            override
            public string ToString() {
                string s = "";
                for (int i = 0; i < graph.Length; i++) {
                    for (int j = 0; j < graph[i].Length; j++) {
                        s += graph[i][j];
                    }
                    s += "\n";
                }
                return s;
            }
        }
    }

}
