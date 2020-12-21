using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace AOC {
    class Day15 {
        private static int[] ParseInput() {
            int[] inp = new int[6];
            using (StreamReader reader = new StreamReader("Inputs/day15.txt")) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    string[] split = line.Split(",");
                    for (int i = 0; i < split.Length; i++) {
                        inp[i] = int.Parse(split[i]);
                    }
                }
            }
            return inp;
        }

        // There might be a pattern but brute forcing it worked...
        public static void SolveDay15_1And2() {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int[] input = ParseInput();
            Dictionary<int, int> numberToLastSpoken = new Dictionary<int, int>();
            int turn, turnLimit, prevNumber, curNumber;
            turn = prevNumber = 0;
            turnLimit = 30000000;
            List<int> a = new List<int>(input);
            for (int i = 1; i <= input.Length; i++)
                numberToLastSpoken[input[i - 1]] = i;
            prevNumber = input[input.Length - 1];
            turn = input.Length + 1;
            while (turn <= turnLimit) {
                if (numberToLastSpoken.ContainsKey(prevNumber)) {
                    curNumber = turn - 1 - numberToLastSpoken[prevNumber];
                } else {
                    curNumber = 0;
                }
                numberToLastSpoken[prevNumber] = turn - 1;
                prevNumber = curNumber;
                turn++;
            }
            sw.Stop();
            Console.WriteLine($"{prevNumber}");
            Console.WriteLine($"The program finished in {sw.ElapsedMilliseconds}ms");
        }
    }

}
