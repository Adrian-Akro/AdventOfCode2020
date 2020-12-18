using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AOC {
    class Day2 {
        private static List<Tuple<int, int, string, string>> ParseInput() {
            List<Tuple<int, int, string, string>> passwords = new List<Tuple<int, int, string, string>>();
            using (StreamReader reader = new StreamReader("Inputs/day2.txt")) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    string[] values = line.Split(" ");
                    passwords.Add(new Tuple<int, int, string, string>(
                        int.Parse(values[0].Split("-")[0]),
                        int.Parse(values[0].Split("-")[1]),
                        values[1].Substring(0, 1),
                        values[2]
                        ));
                }
            }
            return passwords;
        }

        public static void SolveDay2_1() {
            List<Tuple<int, int, string, string>> passwords = ParseInput();
            int valid = 0;
            foreach (Tuple<int, int, string, string> password in passwords) {
                int count = 0;
                for (int i = 0; i < password.Item4.Length; i++) {
                    if (password.Item4.Substring(i, 1).Equals(password.Item3)) count++;
                }
                if (count >= password.Item1 && count <= password.Item2) valid++;
            }
            Console.WriteLine($"There are {valid} valid passwords");
        }

        public static void SolveDay2_2() {
            List<Tuple<int, int, string, string>> passwords = ParseInput();
            int valid = 0;
            foreach (Tuple<int, int, string, string> password in passwords) {
                if ((password.Item4.Substring(password.Item1 - 1, 1).Equals(password.Item3)
                    || password.Item4.Substring(password.Item2 - 1, 1).Equals(password.Item3))
                    && !password.Item4.Substring(password.Item1 - 1, 1).Equals(password.Item4.Substring(password.Item2 - 1, 1))) 
                    valid++;
            }
            Console.WriteLine($"There are {valid} valid passwords");
        }
    }

}
