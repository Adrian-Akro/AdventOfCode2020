using System;
using System.Collections.Generic;
using System.IO;

namespace AOC {
    class Day7 {
        public static readonly string FINAL_BAG = "shiny gold";
        private static Dictionary<string, List<Tuple<string, int>>> ParseInput() {
            Dictionary<string, List<Tuple<string, int>>> ruleset = new Dictionary<string, List<Tuple<string, int>>>();
            using (StreamReader reader = new StreamReader("Inputs/day7.txt")) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    string[] split = line.Split("contain");
                    List<Tuple<string, int>> contents = new List<Tuple<string, int>>();
                    if (int.TryParse(split[1].Trim().Substring(0, 1), out int n)) {
                        foreach (string bag in split[1].Split(",")) {
                            string[] bagSplit = bag.Trim().Substring(1).Trim().Split(" ");
                            contents.Add(new Tuple<string, int>(
                                    $"{bagSplit[0]} {bagSplit[1]}",
                                    int.Parse(bag.Trim().Substring(0, 1))
                                ));
                        }
                    }
                    ruleset.Add(
                        split[0].Split("bags")[0].Trim(),
                        contents);
                }
            }
            return ruleset;
        }

        public static void SolveDay7_1() {
            int count = 0;
            Dictionary<string, List<Tuple<string, int>>> ruleset = ParseInput();
            foreach (string bag in ruleset.Keys) {
                if (!bag.Equals(FINAL_BAG) && CanContainFinalBag(ruleset, bag)) 
                    count++;
            }
            Console.WriteLine($"{count} bags can contain the {FINAL_BAG} colored bag");
        }

        public static void SolveDay7_2() {
            Dictionary<string, List<Tuple<string, int>>> ruleset = ParseInput();
            Console.WriteLine($"{GetBagContentQuantity(ruleset, FINAL_BAG)} bags can contained within the {FINAL_BAG} colored bag");
        }

        private static bool CanContainFinalBag(Dictionary<string, List<Tuple<string, int>>> ruleset, string bag) {
            if (bag.Equals(FINAL_BAG)) 
                return true;
            else if (ruleset[bag].Count == 0) return false;
            else {
                foreach(Tuple<string, int> contents in ruleset[bag]) {
                    if (CanContainFinalBag(ruleset, contents.Item1)) return true;
                }
            }
            return false;
        }

        private static int GetBagContentQuantity(Dictionary<string, List<Tuple<string, int>>> ruleset, string bag) {
            int count = 0;
            foreach (Tuple<string, int> contents in ruleset[bag]) {
                count += (contents.Item2 * GetBagContentQuantity(ruleset, contents.Item1)) + contents.Item2;
            }
            return count;
        }
    }

}
