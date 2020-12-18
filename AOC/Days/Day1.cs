using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AOC {
    class Day1 {

        public static void Solveday1_1() {
            SortedSet<int> ss = new SortedSet<int>();
            using (StreamReader reader = new StreamReader("Inputs/day1.txt")) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    ss.Add(int.Parse(line));
                }
            }
            int[] numbersSorted = new int[ss.Count];
            ss.CopyTo(numbersSorted);
            int primaryIdx = -1;
            int secondaryIdx = -1;
            for (int i = 0; i < numbersSorted.Length; i++) {
                secondaryIdx = day1_1FindMatchingIndex(numbersSorted[i], 2020, numbersSorted);
                if (secondaryIdx >= 0) {
                    primaryIdx = i;
                    break;
                }
            }
            Console.WriteLine($"The result is {numbersSorted[primaryIdx]} + {numbersSorted[secondaryIdx]} = 2020, which multiplied equals {numbersSorted[primaryIdx] * numbersSorted[secondaryIdx]}");
        }

        private static int day1_1FindMatchingIndex(int number, int result, int[] numbersSorted) {
            int secondaryIdx = numbersSorted.Length - 1;
            for (int j = secondaryIdx; j >= 0 && secondaryIdx >= 0 && secondaryIdx < numbersSorted.Length; j /= 2) {
                if (j == 0) secondaryIdx--;
                if (number + numbersSorted[secondaryIdx] > result) secondaryIdx -= j / 2;
                else if (number + numbersSorted[secondaryIdx] < result) secondaryIdx += j / 2;
                else break;
                if (j > 1 && j % 2 != 0) j++;
                else if (j == 0) break;
            }
            if (number + numbersSorted[secondaryIdx] == result) {
                return secondaryIdx;
            }
            return -1;
        }


        public static void Solveday1_2() {
            SortedSet<int> ss = new SortedSet<int>();
            using (StreamReader reader = new StreamReader("Inputs/day1.txt")) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    ss.Add(int.Parse(line));
                }
            }
            int[] numbersSorted = new int[ss.Count];
            ss.CopyTo(numbersSorted);
            int primaryIdx = -1;
            Tuple<int, int> missingIdx = new Tuple<int, int>(-1, -1);
            for (int i = 0; i < numbersSorted.Length; i++) {
                missingIdx = day1_2FindMatchingIndex(numbersSorted[i], 2020, numbersSorted);
                if (missingIdx != null) {
                    primaryIdx = i;
                    break;
                }
            }
            Console.WriteLine($"The result is {numbersSorted[primaryIdx]} + {numbersSorted[missingIdx.Item1]} + {numbersSorted[missingIdx.Item2]} = 2020, which multiplied equals {numbersSorted[primaryIdx] * numbersSorted[missingIdx.Item1] * numbersSorted[missingIdx.Item2]}");
        }

        private static Tuple<int, int> day1_2FindMatchingIndex(int number, int result, int[] numbersSorted) {
            int secondaryIdx = numbersSorted.Length - 1;
            int tertiaryIdx = numbersSorted.Length - 1;
            for (int j = secondaryIdx; j >= 0 && secondaryIdx >= 0 && secondaryIdx < numbersSorted.Length; j /= 2) {
                if (j == 0) secondaryIdx--;
                if (number + numbersSorted[secondaryIdx] > result) secondaryIdx -= j / 2;
                else if (number + numbersSorted[secondaryIdx] < result) {
                    tertiaryIdx = day1_1FindMatchingIndex(number + numbersSorted[secondaryIdx], 2020, numbersSorted);
                    if (tertiaryIdx >= 0) {
                        return new Tuple<int, int>(secondaryIdx, tertiaryIdx);
                    }
                    secondaryIdx += j / 2;
                } else break;
                if (j > 1 && j % 2 != 0) j++;
                else if (j == 0) break;
            }
            return null;
        }

    }
}
