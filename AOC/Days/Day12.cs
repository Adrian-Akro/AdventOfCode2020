using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AOC {

    class Day12 {
        private static List<Tuple<char, int>> ParseInput() {
            List<Tuple<char, int>> input = new List<Tuple<char, int>>();
            using (StreamReader reader = new StreamReader("Inputs/day12.txt")) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    input.Add(new Tuple<char, int>(
                            Convert.ToChar(line.Substring(0, 1)),
                            int.Parse(line.Substring(1))
                        ));
                }
            }
            return input;
        }

        /*
         * The ship stats facing east.
         * Only L and R change the direction the ship is facing.
         * Forward moves the ship in the direction it is facing.
         * R and L instructions provide the turning degrees in its value.
         * The rest of the instructions provide the amount of squares the ship should move in the grid.
         */
        public static void SolveDay12_1() {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<Tuple<char, int>> values = ParseInput();
            int deg = 0;
            Dictionary<Direction, int> coordinateMovement = new Dictionary<Direction, int> {
                { Direction.North, 0 },
                { Direction.South, 0 },
                { Direction.East, 0 },
                { Direction.West, 0 }
            };
            foreach (Tuple<char, int> value in values) {
                switch ((Direction)value.Item1) {
                    case Direction.Left:
                        deg -= value.Item2;
                        break;
                    case Direction.Right:
                        deg += value.Item2;
                        break;
                    case Direction.Forward:
                        coordinateMovement[GetFacingDirection(deg)] += value.Item2;
                        break;
                    default:
                        coordinateMovement[(Direction)value.Item1] += value.Item2;
                        break;
                }
            }
            sw.Stop();
            Console.WriteLine($"The ship moves {coordinateMovement[Direction.North] - coordinateMovement[Direction.South]} units north " +
                $"and {coordinateMovement[Direction.East] - coordinateMovement[Direction.West]} units east " +
                $"resulting in a Manhattan distance of " +
                $"{Math.Abs(coordinateMovement[Direction.North] - coordinateMovement[Direction.South]) + Math.Abs(coordinateMovement[Direction.East] - coordinateMovement[Direction.West])}");
            Console.WriteLine($"The program finished in {sw.ElapsedMilliseconds}ms");
        }        
        
        public static void SolveDay12_2() {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<Tuple<char, int>> values = ParseInput();
            Dictionary<Direction, int> shipCoordinates = new Dictionary<Direction, int> {
                { Direction.North, 0 },
                { Direction.South, 0 },
                { Direction.East, 0 },
                { Direction.West, 0 }
            };
            Dictionary<Direction, int> waypointCoordinates = new Dictionary<Direction, int> {
                { Direction.North, 1 },
                { Direction.South, 0 },
                { Direction.East, 10 },
                { Direction.West, 0 }
            };
            foreach (Tuple<char, int> value in values) {
                switch ((Direction)value.Item1) {
                    case Direction.Left:
                        waypointCoordinates = ComputeWayPointSpin(-value.Item2, waypointCoordinates);
                        break;
                    case Direction.Right:
                        waypointCoordinates = ComputeWayPointSpin(value.Item2, waypointCoordinates);
                        break;
                    case Direction.Forward:
                        shipCoordinates = MoveTowardsWaypoint(shipCoordinates, waypointCoordinates, value.Item2);
                        break;
                    default:
                        waypointCoordinates[(Direction)value.Item1] += value.Item2;
                        break;
                }
            }
            sw.Stop();
            Console.WriteLine($"The ship moves {shipCoordinates[Direction.North] - shipCoordinates[Direction.South]} units north " +
                $"and {shipCoordinates[Direction.East] - shipCoordinates[Direction.West]} units east " +
                $"resulting in a Manhattan distance of " +
                $"{Math.Abs(shipCoordinates[Direction.North] - shipCoordinates[Direction.South]) + Math.Abs(shipCoordinates[Direction.East] - shipCoordinates[Direction.West])}");
            Console.WriteLine($"The program finished in {sw.ElapsedMilliseconds}ms");
        }

        enum Direction {
            North = 'N',
            South = 'S',
            West = 'W',
            East = 'E',
            Left = 'L',
            Right = 'R',
            Forward = 'F'
        }

        private static Direction GetFacingDirection(int degrees) {
            degrees %= 360;
            if (degrees < 0) {
                degrees += 360 * (int)Math.Ceiling(degrees / -360d);
            }
            if (degrees % 360 == 0) return Direction.East;
            else if (degrees % 270 == 0) return Direction.North;
            else if (degrees % 180 == 0) return Direction.West;
            else if (degrees % 90 == 0) return Direction.South;
            throw new InvalidOperationException();
        }

        private static Dictionary<Direction, int> ComputeWayPointSpin(int degrees, Dictionary<Direction, int> waypointCoordinates) {
            int aux1, aux2, noOfRightRuns;
            if (degrees < 0) {
                degrees += 360 * (int)Math.Ceiling(degrees / -360d);
            }
            noOfRightRuns = degrees / 90;
            for (int i = 0; i < noOfRightRuns; i++) {
                aux1 = waypointCoordinates[Direction.North];
                aux2 = waypointCoordinates[Direction.South];
                waypointCoordinates[Direction.North] = waypointCoordinates[Direction.West];
                waypointCoordinates[Direction.South] = waypointCoordinates[Direction.East];
                waypointCoordinates[Direction.East] = aux1;
                waypointCoordinates[Direction.West] = aux2;
            }
            return waypointCoordinates;
        }

        private static Dictionary<Direction, int> MoveTowardsWaypoint(Dictionary<Direction, int> shipCoordinates, Dictionary<Direction, int> waypointCoordinates, int noOfMovements) {
            shipCoordinates[Direction.North] += waypointCoordinates[Direction.North] * noOfMovements;
            shipCoordinates[Direction.South] += waypointCoordinates[Direction.South] * noOfMovements;
            shipCoordinates[Direction.East] += waypointCoordinates[Direction.East] * noOfMovements;
            shipCoordinates[Direction.West] += waypointCoordinates[Direction.West] * noOfMovements;
            return shipCoordinates;
        }
    }

}
