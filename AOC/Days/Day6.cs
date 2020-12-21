using System;
using System.Collections.Generic;
using System.IO;

namespace AOC {
    class Day6 {
        public static void SolveDay6_1() {
            HashSet<char> stringSet = new HashSet<char>();
            int count = 0;
            using (StreamReader reader = new StreamReader("Inputs/day6.txt")) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    if (line.Equals("")) {
                        count += stringSet.Count;
                        stringSet.Clear();
                    } else {
                        foreach (char s in line) {
                            stringSet.Add(s);
                        }
                    }
                }
            }

            Console.WriteLine($"The sum of the counts is {count}");
        }

        public static void SolveDay6_2() {
            Dictionary<char, int> answerDict = new Dictionary<char, int>();
            int count = 0;
            int members = 0;
            using (StreamReader reader = new StreamReader("Inputs/day6.txt")) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    if (line.Equals("")) {
                        foreach (int answerCount in answerDict.Values) {
                            if (answerCount == members) count++;
                        }
                        answerDict.Clear();
                        members = 0;
                    } else {
                        members++;
                        foreach (char s in line) {
                            if (answerDict.ContainsKey(s)) answerDict[s]++;
                            else answerDict.Add(s, 1);
                        }
                    }
                }
            }

            Console.WriteLine($"The sum of the counts is {count}");
        }
    }

}
