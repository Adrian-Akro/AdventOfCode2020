using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AOC {
    class Day16 {
        private static (Dictionary<string, List<int>>, int[], LinkedList<int[]>) ParseInput() {
            Dictionary<string, List<int>> fieldToValueRanges = new Dictionary<string, List<int>>();
            int[] myTicketValues = null;
            // We use a linked list because the operations dont require us to look up specific indexes
            // but delete elements in the middle and iterate over every element
            // Ideally we could use a node from a doubly linked list that support remove since
            // we remove while iterating
            LinkedList<int[]> othersTicketValues = new LinkedList<int[]>();
            int dataProcessingStep = 0;
            using (StreamReader reader = new StreamReader("Inputs/day16.txt")) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    if (dataProcessingStep > 0 && line.Contains(":")) continue;
                    if (line.Equals("")) {
                        dataProcessingStep++;
                        continue;
                    }
                    switch (dataProcessingStep) {
                        case 0:
                            string[] keyAndValueRanges = line.Split(":");
                            string[] valueRanges = keyAndValueRanges[1].Split("or");
                            fieldToValueRanges[keyAndValueRanges[0]] = new List<int>();
                            foreach (string range in valueRanges)
                                fieldToValueRanges[keyAndValueRanges[0]].AddRange(range.Split("-").Select(x => int.Parse(x.Trim())));
                            break;
                        case 1:
                            myTicketValues = line.Split(",").Select(x => int.Parse(x.Trim())).ToArray();
                            break;
                        case 2:
                            othersTicketValues.AddLast(line.Split(",").Select(x => int.Parse(x.Trim())).ToArray());
                            break;
                        default:
                            break;
                    }
                }
            }
            return (fieldToValueRanges, myTicketValues, othersTicketValues);
        }

        public static void SolveDay16_1() {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Dictionary<string, List<int>> fieldToValueRanges = new Dictionary<string, List<int>>();
            LinkedList<int[]> othersTicketValues = new LinkedList<int[]>();
            (fieldToValueRanges, _, othersTicketValues) = ParseInput();
            int sum = 0;
            foreach (int[] ticket in othersTicketValues)
                foreach (int value in ticket)
                    if (!IsWithinRange(value, fieldToValueRanges)) sum += value;
            sw.Stop();
            Console.WriteLine($"{sum}");
            Console.WriteLine($"The program finished in {sw.ElapsedMilliseconds}ms");
        }

        public static void SolveDay16_2() {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Dictionary<string, List<int>> fieldToValueRanges = new Dictionary<string, List<int>>();
            LinkedList<int[]> othersTicketValues = new LinkedList<int[]>();
            int[] myTicket;
            (fieldToValueRanges, myTicket, othersTicketValues) = ParseInput();
            HashSet<string>[] suitableKeysPerIndex = new HashSet<string>[myTicket.Length];
            LinkedListNode<int[]> ticket = othersTicketValues.First;
            // First we load the possible values with the ranges in my ticket
            for (int i = 0; i < myTicket.Length; i++) {
                suitableKeysPerIndex[i] = new HashSet<string>();
                foreach (string value in GetSuitableRangeKeys(myTicket[i], fieldToValueRanges))
                    suitableKeysPerIndex[i].Add(value);
            }

            do {
                LinkedListNode<int[]> ticketToRemove = ticket;
                ticket = ticket.Next;
                foreach (int value in ticketToRemove.Value)
                    if (!IsWithinRange(value, fieldToValueRanges)) {
                        // We do this because on removing a ticket its prev and next values are set to null
                        othersTicketValues.Remove(ticketToRemove);
                        break;
                    }
            } while (ticket != null);
            ticket = othersTicketValues.First;
            do {
                foreach (int value in ticket.Value)
                    for (int i = 0; i < ticket.Value.Length; i++) {
                        // if we already found what the key for that index is we do not check again
                        if (suitableKeysPerIndex[i].Count <= 1) continue;
                        HashSet<string> suitableValues = GetSuitableRangeKeys(ticket.Value[i], fieldToValueRanges);
                        // Could be made faster by filtering manually
                        suitableKeysPerIndex[i] = new HashSet<string>(suitableKeysPerIndex[i].Where(x => suitableValues.Contains(x)));
                    }
            } while ((ticket = ticket.Next) != null && suitableKeysPerIndex.Any(x => x.Count > 1));
            string[] computedKeys = ComputeSuitableValues(suitableKeysPerIndex);
            long product = 1;
            for (int i = 0; i < myTicket.Length; i++)
                if (computedKeys[i].StartsWith("departure"))
                    product *= myTicket[i];
            sw.Stop();
            Console.WriteLine($"The product of the values of my ticket starting with departure is {product}");
            Console.WriteLine($"The program finished in {sw.ElapsedMilliseconds}ms");
        }

        // Ranges are a bidimensional an even number of entries, the first of each pair is the lower boundary and the last the upper boundary
        private static bool IsWithinRange(int number, Dictionary<string, List<int>> fieldToValueRanges) {
            foreach (KeyValuePair<string, List<int>> kvp in fieldToValueRanges)
                for (int i = 0; i < kvp.Value.Count; i += 2)
                if (number >= kvp.Value[i] && number <= kvp.Value[i+1]) 
                    return true;
            return false;
        }

        // Though a bit redundant the above is a faster for just checking
        private static HashSet<string> GetSuitableRangeKeys(int number, Dictionary<string, List<int>> fieldToValueRanges) {
            HashSet<string> keys = new HashSet<string>();
            foreach (KeyValuePair<string, List<int>> kvp in fieldToValueRanges)
                for (int i = 0; i < kvp.Value.Count; i += 2)
                    if (number >= kvp.Value[i] && number <= kvp.Value[i + 1])
                        keys.Add(kvp.Key);
            return keys;
        }

        private static string[] ComputeSuitableValues(HashSet<string>[] currentSuitableValues) {
            string[] values = new string[currentSuitableValues.Length];
            HashSet<string> found = new HashSet<string>(currentSuitableValues.Length);
            for (int i = 0; i < currentSuitableValues.Length; i++) {
                for (int j = 0; j < currentSuitableValues.Length; j++) {
                    // We cannot figure out the proper solution so we continue
                    if (currentSuitableValues[j].Count != found.Count + 1) continue;
                    foreach (string key in currentSuitableValues[j])
                        if (!found.Contains(key)) {
                            found.Add(key);
                            values[j] = key;
                            break;
                        }
                }
            }
            return values;
        }
    }

}
