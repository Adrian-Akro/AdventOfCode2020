using System;
using System.IO;

namespace AOC {
    class Day3 {
        private static Grid ParseInput() {
            Grid grid = new Grid(323,31);
            using (StreamReader reader = new StreamReader("Inputs/day3.txt")) {
                string line;
                int y = 0;
                while ((line = reader.ReadLine()) != null) {
                    for (int x = 0; x < line.Length; x++) {
                        grid.Insert(y, x, line.Substring(x, 1));
                    }
                    y++;
                }
            }
            return grid;
        }

        public static void SolveDay3_1() {
            Grid grid = ParseInput();
            int x, y, count;
            x = y = count = 0;
            while (grid.IsYWithinBounds(++y)) {
                if (grid.GetValue(y, x += 3) == Grid.TREE_TOKEN) count++;
            }
            Console.WriteLine($"We found {count} trees");
        }

        public static void SolveDay3_2() {
            Grid grid = ParseInput();
            int x, y;
            int[] count = new int[5];
            Tuple<int, int>[] pathingWays = new Tuple<int, int>[5] {
                new Tuple<int, int>(1, 1),
                new Tuple<int, int>(1, 3),
                new Tuple<int, int>(1, 5),
                new Tuple<int, int>(1, 7),
                new Tuple<int, int>(2, 1),
            };
            for (int i = 0; i < pathingWays.Length; i++) {
                Tuple<int, int> way = pathingWays[i];
                x = y = 0;
                while (grid.IsYWithinBounds(y += way.Item1)) {
                    if (grid.GetValue(y, x += way.Item2) == Grid.TREE_TOKEN) count[i]++;
                }
            }
            Console.WriteLine($"We found {count[0]}, {count[1]}, {count[2]}, {count[3]}, {count[4]} trees which multiplied equal {count[0] * count[1] * count[2] * count[3] * count[4]}");
        }

        class Grid {
            string[][] graph;
            public static readonly string TREE_TOKEN = "#";
            public static readonly string PATH_TOKEN = ".";

            public Grid(int dimY, int dimX) {
                graph = new string[dimY][];
                for (int i = 0; i < dimY; i++) {
                    graph[i] = new string[dimX];
                }
            }

            public void Insert(int y, int x, string value) {
                if (!value.Equals(TREE_TOKEN) && !value.Equals(PATH_TOKEN)) return;
                graph[y][x] = value;
            }

            public string GetValue(int y, int x) {
                return graph[y][x % graph[y].Length];
            }

            public bool IsYWithinBounds(int y) {
                if (y < graph.Length) return true;
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
