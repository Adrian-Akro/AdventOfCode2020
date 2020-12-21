using System;
using System.Diagnostics;
using System.IO;

namespace AOC {
    class Day_ {
        private static void ParseInput() {
            using (StreamReader reader = new StreamReader("Inputs/day.txt")) {
                string line;
                while ((line = reader.ReadLine()) != null) { }
            }
        }

        public static void Solve() {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            sw.Stop();
            Console.WriteLine($"The program finished in {sw.ElapsedMilliseconds}ms");
        }
    }

}
