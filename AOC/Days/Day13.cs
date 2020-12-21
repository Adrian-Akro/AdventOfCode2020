using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace AOC {
    class Day13 {
        public const int NO_BUS = -1;
        private static (int, int[]) ParseInput() {
            int earliestDeparture;
            int[] busIds;
            using (StreamReader reader = new StreamReader("Inputs/day13.txt")) {
                earliestDeparture = int.Parse(reader.ReadLine());
                busIds = reader.ReadLine().Split(",")
                    .Select(strId => {
                        if (strId.All(char.IsNumber)) return int.Parse(strId);
                        return NO_BUS;
                    })
                    .ToArray();
            }
            return (earliestDeparture, busIds);
        }

        public static void SolveDay13_1() {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int earliestDeparture;
            int[] busIds;
            int currentOptimalBus, currentWaitMinutes, waitMinutes;
            currentWaitMinutes = int.MaxValue;
            currentOptimalBus = -1;
            (earliestDeparture, busIds) = ParseInput();
            foreach (int bus in busIds) {
                if (bus == NO_BUS) continue;
                waitMinutes = bus - (earliestDeparture % bus);
                if (waitMinutes < currentWaitMinutes) {
                    currentOptimalBus = bus;
                    currentWaitMinutes = waitMinutes;
                }
            }
            sw.Stop();
            Console.WriteLine($"You will have to wait {currentWaitMinutes} minutes for the bus {currentOptimalBus} to depart, which multiplied equals {currentOptimalBus * currentWaitMinutes}");
            Console.WriteLine($"The program finished in {sw.ElapsedMilliseconds}ms");
        }

        public static void SolveDay13_2() {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int curIndex = 0;
            ulong curIter, incrementValue;
            curIter = incrementValue = 0;
            int[] busIds;
            (_, busIds) = ParseInput();
            for (int i = 0; i < busIds.Length; i++)
                if (busIds[i] > 0) {
                    curIndex = i;
                    incrementValue = Convert.ToUInt64(busIds[i]);
                    break;
                }
            while (curIndex + 1 < busIds.Length) {
                curIter += incrementValue;
                int nextItemPos = GetNextItem(busIds, curIndex);
                if ((curIter + Convert.ToUInt64(nextItemPos)) % Convert.ToUInt64(busIds[nextItemPos]) == 0) {
                    curIndex = nextItemPos;
                    incrementValue *= Convert.ToUInt64(busIds[nextItemPos]);
                    Console.WriteLine($"Solution found for {busIds[nextItemPos]} at {curIter}");
                }
            }
            sw.Stop();
            Console.WriteLine($"The program finished in {sw.ElapsedMilliseconds}ms");
        }

        private static int GetNextItem(int[] busIds, int curIndex) {
            if (curIndex +1 < busIds.Length)
                for (int i = curIndex + 1; i < busIds.Length; i++)
                    if (busIds[i] > 0) return i;
            return -1;
        }
    }

}
