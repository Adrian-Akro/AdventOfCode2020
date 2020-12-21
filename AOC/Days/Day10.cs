using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace AOC {
    class Day10 {
        private static List<int> ParseInput() {
            List<int> numbers = new List<int>();
            using (StreamReader reader = new StreamReader("Inputs/day10.txt")) {
                string line;
                while ((line = reader.ReadLine()) != null) numbers.Add(int.Parse(line));
            }
            return numbers;
        }

        public static void SolveDay10_1() {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<int> adapterRatings = ParseInput();
            // The first rating is always 0
            adapterRatings.Add(0);
            adapterRatings.Sort();
            // Your adapter is included and is always 3 jolts higher
            adapterRatings.Add(adapterRatings[adapterRatings.Count - 1] + 3);
            int[] jumps = CountJumps(adapterRatings);
            sw.Stop();
            Console.WriteLine($"Number of size 1 jumps {jumps[0]}, " +
                $"number of size 2 jumps {jumps[1]}, " +
                $"number of size 3 jumps {jumps[2]}. " +
                $"The result of multiplying the size 1 and 3 jumps is {jumps[0] * jumps[2]}");
            Console.WriteLine($"The program finished in {sw.ElapsedMilliseconds}ms");
        }

        public static void SolveDay10_2() {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<int> adapterRatings = ParseInput();
            adapterRatings.Add(0);
            adapterRatings.Sort();
            long[] dynArrangementResults = new long[adapterRatings[adapterRatings.Count - 1] + 1];
            // We start with one base arrangement
            dynArrangementResults[0] = 1;
            for (int i = 1; i < adapterRatings.Count; i++) {
                int cur = adapterRatings[i];
                List<int> outwardEdges = GetEdgesTo(i, adapterRatings);
                List<int> inwardEdges = GetEdgesFrom(i, adapterRatings);
                long sum = 0;
                foreach (int edge in inwardEdges) {
                    sum += dynArrangementResults[edge];
                }
                dynArrangementResults[cur] = sum;
                foreach (int edge in outwardEdges) {
                    // Since the edges are sorted in such a way that the lowest gets computed first
                    // and we know that if there exists a path i - j for every node k between i and j
                    // there will exist a path i - k - j and also for every iteration we know that another
                    // node k is added to the chain after the last k and we also know that there must exist a connection from i to the new k so
                    // we need to multiply the sum for every additional node k, given that i is the current node, j is the highest valued edge
                    // and k is every node inbetween
                    if (edge - cur > 1) sum >>= 1;
                    dynArrangementResults[edge] += sum;
                }
            };
            sw.Stop();
            Console.WriteLine($"There are {dynArrangementResults[dynArrangementResults.Length - 1]} possible arrangements");
            Console.WriteLine($"The program finished in {sw.ElapsedMilliseconds}ms");
        }

        public static void SolveDay10_2Extra() {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<int> adapterRatings = ParseInput();
            adapterRatings.Add(0);
            adapterRatings.Sort();
            int endpoint = adapterRatings[adapterRatings.Count - 1];
            long[] dynArrangementResults = new long[endpoint + 1];
            int[] valueToIndex = new int[endpoint + 1];
            bool[] visited = new bool[endpoint + 1];
            bool[] enqueued = new bool[endpoint + 1];
            List<int>[] inwardEdges = new List<int>[adapterRatings.Count];
            List<int>[] outwardEdges = new List<int>[adapterRatings.Count];
            Queue<int> bfsRevisitQueue = new Queue<int>();
            for (int i = 0; i < adapterRatings.Count; i++) {
                valueToIndex[adapterRatings[i]] = i;
                inwardEdges[i] = GetEdgesFrom(i, adapterRatings);
                outwardEdges[i] = GetEdgesTo(i, adapterRatings);
            }
            bfsRevisitQueue.Enqueue(endpoint);
            // We start with one base arrangement
            dynArrangementResults[endpoint] = 1;
            visited[endpoint] = true;
            while (bfsRevisitQueue.Count > 0) {
                int cur = bfsRevisitQueue.Dequeue();
                enqueued[cur] = false;
                // Check if a vertex is ready to be computed, else we add it back to the end of the queue
                if (!IsFullyVisited(valueToIndex[cur], visited, outwardEdges[valueToIndex[cur]])) {
                    bfsRevisitQueue.Enqueue(cur);
                    continue;
                }
                foreach (int edge in inwardEdges[valueToIndex[cur]]) {
                    dynArrangementResults[edge] += dynArrangementResults[cur];
                    if (!visited[edge] && !enqueued[edge]) {
                        bfsRevisitQueue.Enqueue(edge);
                        enqueued[edge] = true;
                    }
                }
                visited[cur] = true;
            }
            sw.Stop();
            Console.WriteLine($"There are {dynArrangementResults[0]} possible arrangements");
            Console.WriteLine($"The program finished in {sw.ElapsedMilliseconds}ms");
        }

        private static bool IsFullyVisited(int i, bool[] visited, List<int> outwardEdges) {
            foreach (int edge in outwardEdges) {
                if (!visited[edge]) {
                    return false;
                }
            }
            return true;
        }

        private static List<int> GetEdgesTo(int i, List<int> adapterRatings) {
            List<int> edges = new List<int>();
            int j = i;
            while (++j < adapterRatings.Count && adapterRatings[j] - adapterRatings[i] <= 3) edges.Add(adapterRatings[j]);
            return edges;
        }

        private static List<int> GetEdgesFrom(int i, List<int> adapterRatings) {
            List<int> edges = new List<int>();
            int j = i;
            while (--j >= 0 && adapterRatings[i] - adapterRatings[j] <= 3) edges.Add(adapterRatings[j]);
            return edges;
        }



        private static int[] CountJumps(List<int> adapterRatings) {
            int[] result = new int[3];
            int prevRating = adapterRatings[0];
            for (int i = 1; i < adapterRatings.Count; i++) {
                result[adapterRatings[i] - prevRating - 1]++;
                prevRating = adapterRatings[i];
            }
            return result;
        }

    }

}



   



