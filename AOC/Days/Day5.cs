using System;
using System.Collections.Generic;
using System.IO;

namespace AOC {
    class Day5 {
        public static void SolveDay5_1() {
            int max = -1;
            using (StreamReader reader = new StreamReader("Inputs/day5.txt")) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    int row = DecodeRow(line.Substring(0, 7));
                    int col = DecodeCol(line.Substring(7, 3));
                    max = Math.Max(max, row * 8 + col);
                }
            }
            Console.WriteLine($"The highest ID is {max}");
        }

        public static void SolveDay5_2() {
            SortedSet<int> ss = new SortedSet<int>();
            int myId = default;
            using (StreamReader reader = new StreamReader("Inputs/day5.txt")) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    ss.Add(DecodeRow(line.Substring(0, 7)) * 8 + DecodeCol(line.Substring(7, 3)));
                }
                foreach (int id in ss) {
                    if (myId != default && id - myId > 1) {
                        break;
                    }
                    myId = id;
                }
            }
            Console.WriteLine($"My ID is {++myId}");
        }

        private static int DecodeRow(string line) {
            int boundary;
            boundary = 0;
            for (int i = 0; i < line.Length; i++) {
                if (line.Substring(i, 1).Equals("B")) boundary += (1 << line.Length) >> (i + 1);
            }
            return boundary;
        }


        private static int DecodeCol(string line) {
            int boundary;
            boundary = 0;
            for (int i = 0; i < line.Length; i++) {
                if (line.Substring(i, 1).Equals("R")) boundary += (1 << line.Length) >> (i + 1);
            }
            return boundary;
        }

    }

}
