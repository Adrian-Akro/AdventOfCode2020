using System;
using System.Collections.Generic;
using System.IO;

namespace AOC {
    class Day8 {
        private static List<Tuple<string, int>> ParseInput() {
            List<Tuple<string, int>> instructions = new List<Tuple<string, int>>();
            using (StreamReader reader = new StreamReader("Inputs/day8.txt")) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    string[] split = line.Split(" ");
                    instructions.Add(new Tuple<string, int>(split[0], int.Parse(split[1])));
                }
            }
            return instructions;
        }

        public static void SolveDay8_1() {
            int accumulator = 0;
            List<Tuple<string, int>> instructions = ParseInput();
            HashSet<int> executedInstructions = new HashSet<int>();
            for (int i = 0; i < instructions.Count;) {
                if (executedInstructions.Contains(i)) break;
                executedInstructions.Add(i);
                switch (instructions[i].Item1) {
                    case "jmp":
                        i += instructions[i].Item2;
                        break;
                    case "acc":
                        accumulator += instructions[i].Item2;
                        i++;
                        break;
                    default:
                        i++;
                        break;
                }
            }
            Console.WriteLine($"The value of the accumulator is {accumulator}");
        }


        public static void SolveDay8_2() {
            List<Tuple<string, int>> instructions = ParseInput();
            HashSet<int> executedInstructions = new HashSet<int>();
            Stack<int> possibleBadInstructions = new Stack<int>();
            int accumulator, increment;
            accumulator = increment = 0;
            for (int i = 0; i < instructions.Count; i += increment) {
                if (executedInstructions.Contains(i)) break;
                executedInstructions.Add(i);
                increment = 1;
                switch(instructions[i].Item1) {
                    case "acc":
                        accumulator += instructions[i].Item2;
                        break;
                    case "jmp":
                        increment = instructions[i].Item2;
                        possibleBadInstructions.Push(i);
                        break;
                    default:
                        possibleBadInstructions.Push(i);
                        break;
                }
            }

            while (possibleBadInstructions.Count > 0
                && (accumulator = TryInstructions(instructions, possibleBadInstructions.Pop())) == -1);

            Console.WriteLine($"The program finished with an accumulated value of {accumulator}");
        }

        private static int TryInstructions(List<Tuple<string, int>> instructions, int instructionToChange) {
            HashSet<int> executedInstructions = new HashSet<int>();
            int accumulator, increment, i;
            accumulator = increment = 0;
            for (i = 0; i < instructions.Count; i += increment) {
                if (executedInstructions.Contains(i)) return -1;
                executedInstructions.Add(i);
                increment = 1;
                switch (i != instructionToChange 
                    ? instructions[i].Item1 
                    : OppositeInstruction(instructions[i].Item1)
                ) {
                    case "acc":
                        accumulator += instructions[i].Item2;
                        break;
                    case "jmp":
                        increment = instructions[i].Item2;
                        break;
                    default:
                        break;
                }
            }
            if (i >= 0 && i <= instructions.Count) return accumulator;
            return -1;
        }

        private static string OppositeInstruction(string instruction) {
            if (instruction.Equals("jmp")) return "nop";
            else if (instruction.Equals("nop")) return "jmp";
            return "acc";
        }

    }

}
