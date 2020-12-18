using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AOC {
    class Day9 {
        // Numbers after 25 first are a sum of the first 25.
        private static List<long> ParseInput() {
            List<long> numbers = new List<long>();
            using (StreamReader reader = new StreamReader("Inputs/day9.txt")) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    numbers.Add(long.Parse(line));
                }
            }
            return numbers;
        }

        public static void SolveDay9_1() {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int rangeOfSum = 25;
            List<long> numbers = ParseInput();
            List<long> pivotNumbers;
            for (int i = rangeOfSum; i < numbers.Count; i++) {
                pivotNumbers = numbers.GetRange(i - rangeOfSum, rangeOfSum);
                if (!CheckNumericSum(pivotNumbers, numbers[i])) {
                    Console.WriteLine($"Number {numbers[i]} does not follow the rule");
                    break;
                }
            }
            sw.Stop();
            Console.WriteLine($"The program finished in {sw.ElapsedMilliseconds}ms");
        }

        public static void SolveDay9_2() {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<long> numbers = ParseInput();
            Queue<long> contiguousSet = new Queue<long>();
            long currentSum, putridInput, lastNumber;
            putridInput = 373803594;
            currentSum = lastNumber = 0;
            for (int i = 0; i < numbers.Count && currentSum != putridInput; i++) {
                contiguousSet.Enqueue(numbers[i]);
                lastNumber = numbers[i];
                currentSum += numbers[i];
                while (currentSum > putridInput && contiguousSet.Count > 0) 
                    currentSum -= contiguousSet.Dequeue();
            }
            long[] sortedSet = contiguousSet.ToArray();
            Array.Sort(sortedSet);
            Console.WriteLine($"The smallest number of the contiguous set is {sortedSet[0]} and the biggest is {sortedSet[sortedSet.Length - 1]} whose sum equals {sortedSet[0] + sortedSet[sortedSet.Length - 1]}");
            sw.Stop();
            Console.WriteLine($"The program finished in {sw.ElapsedMilliseconds}ms");
        }

        private static bool CheckNumericSum(List<long> sumOfNumbers, long number) {
            sumOfNumbers.Sort();
            int j, count;
            for (int i = 0; i < sumOfNumbers.Count && sumOfNumbers[i] + sumOfNumbers[0] < number; i++) {
                j = count = sumOfNumbers.Count - 1;
                while (count > 0 && j < sumOfNumbers.Count) {
                    if (count % 2 != 0 && count > 1) count++;
                    count >>= 1;
                    if (sumOfNumbers[i] + sumOfNumbers[j] > number) j -= count;
                    else if (sumOfNumbers[i] + sumOfNumbers[j] < number) j += count;
                    else return true;
                }
            }
            return false;
        }


    }

}
