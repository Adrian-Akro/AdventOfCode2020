using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace AOC {
    class Day14 {
        private static Dictionary<int, (string, int)> ParseInputPart1() {
            Dictionary<int, (string, int)> mapValues = new Dictionary<int, (string, int)>();
            using (StreamReader reader = new StreamReader("Inputs/day14.txt")) {
                string line;
                string curKey = "";
                string[] split;
                while ((line = reader.ReadLine()) != null) {
                    split = line.Split("=");
                    if (split[0].Trim().Equals("mask")) {
                        curKey = split[1].Trim();
                    } else {
                        int memoStart = split[0].IndexOf("[") + 1;
                        int memoEnd = split[0].IndexOf("]");
                        mapValues[int.Parse(split[0].Substring(memoStart, memoEnd - memoStart))] = (curKey, int.Parse(split[1].Trim()));
                    }
                }
            }
            return mapValues;
        }

        private static Dictionary<ulong, int> ParseInputPart2() {
            Dictionary<ulong, int> mapValues = new Dictionary<ulong, int>();
            using (StreamReader reader = new StreamReader("Inputs/day14.txt")) {
                string line;
                string curKey = "";
                string[] split;
                while ((line = reader.ReadLine()) != null) {
                    split = line.Split("=");
                    if (split[0].Trim().Equals("mask")) {
                        curKey = split[1].Trim();
                    } else {
                        int memoStart = split[0].IndexOf("[") + 1;
                        int memoEnd = split[0].IndexOf("]");
                        List<ulong> addresses = GetMappedValuesP2(curKey, int.Parse(split[0].Substring(memoStart, memoEnd - memoStart)));
                        foreach (ulong adr in addresses)
                            mapValues[adr] = int.Parse(split[1].Trim());
                    }
                }
            }
            return mapValues;
        }

        public static void SolveDay14_1() {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Dictionary<int, (string, int)> memoValues = ParseInputPart1();
            ulong sum = 0;
            foreach (KeyValuePair<int, (string, int)> kvp in memoValues)
                    sum +=GetMappedValueP1(kvp.Value.Item1, kvp.Value.Item2);
            sw.Stop();
            Console.WriteLine($"The sum is {sum}");
            Console.WriteLine($"The program finished in {sw.ElapsedMilliseconds}ms");
        }

        public static void SolveDay14_2() {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Dictionary<ulong, int> memoValues = ParseInputPart2();
            ulong sum = 0;
            foreach (KeyValuePair<ulong, int> kvp in memoValues)
                sum += Convert.ToUInt64(kvp.Value);
            sw.Stop();
            Console.WriteLine($"The sum is {sum}");
            Console.WriteLine($"The program finished in {sw.ElapsedMilliseconds}ms");
        }

        private static ulong SetBit(ulong number, int position) { return (1UL << position) | number; }
        private static ulong ClearBit(ulong number, int position) { return number & (~(1UL << (position))); }

        private static ulong GetMappedValueP1(string map, int initialValue) {
            ulong value = Convert.ToUInt64(initialValue);
            for (int i = 0; i < map.Length; i++) {
                switch (map[map.Length - 1 - i]) {
                    case '1':
                        value = SetBit(value, i);
                        break;
                    case '0':
                        value = ClearBit(value, i);
                        break;
                    default:
                        break;
                }
            }
            return (ulong)value;
        }

        private static List<ulong> GetMappedValuesP2(string map, int initialValue) {
            List<ulong> vals = new List<ulong>();
            vals.Add(Convert.ToUInt64(initialValue));
            for (int i = 0; i < map.Length; i++) {
                int size = vals.Count;
                for (int j = 0; j < size; j++)
                    switch (map[map.Length - 1 - i]) {
                        case '1':
                            vals[j] = SetBit(vals[j], i);
                            break;
                        case 'X':
                            vals[j] = SetBit(vals[j], i);
                            vals.Add(ClearBit(vals[j], i));
                            break;
                        default:
                            break;
                    }
            }
            return vals;
        }
    }

}
