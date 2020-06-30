using System;
using System.Collections.Generic;

namespace NQueens
{
    static class SimAnnealingNQueensTest
    {
        private static int[] maxTemps = new int[8] {1, 10, 100, 1000, 10000, 100000, 1000000, 10000000};
        private static double[] decrements = new double[10] {0.91, 0.92, 0.93, 0.94, 0.95, 0.96, 0.97, 0.98, 0.99, 0.999};
        private static double[] threshes = new double[10] {0.5, 0.4, 0.3, 0.25, 0.2, 0.15, 0.1, 0.05, 0.01, 0.001};
        private static List<long> iterations;
        private static List<double> timings;
        private static List<string> results;

        public static void RunTests(int size, int runs)
        {
            BoardState board = new BoardState(size);
            results = new List<string>();

            for (int i = 0; i < runs; i++)
            {
                Console.WriteLine();
                Console.WriteLine("----------------------------");
                Console.WriteLine("----- TESTS " + (i+1) + " STARTING -----");
                Console.WriteLine("----------------------------");
                Console.WriteLine();

                iterations = new List<long>();
                timings = new List<double>();
                BoardState minState = board;

                foreach (var temp in maxTemps)
                {
                    Console.WriteLine("Running Tests with Max Temperature of " + temp + ": ");

                    foreach (var dec in decrements)
                    {
                        Console.WriteLine("\tRunning Tests with decrement of " + dec + ": ");

                        foreach (var thresh in threshes)
                        {
                            Console.WriteLine("\t\tRunning Tests with threshold of " + thresh + ": ");
                            BoardState finalState = new BoardState(size);
                            long average = 0;
                            double time = 0;

                            for (int k = 0; k < 10; k++)
                            {
                                finalState = SimAnnealingNQueens.startSimAnneal(board, temp, dec, thresh);

                                if (finalState.heuristic < minState.heuristic)
                                {
                                    minState = finalState;
                                }

                                average += SimAnnealingNQueens.iteration;
                                time += SimAnnealingNQueens.seconds;
                            }

                            average /= 10;
                            time /= 10.0;

                            iterations.Add(average);
                            timings.Add(time);
                        }
                    }
                }

                int mindex = 0;

                for (int k = 0; k < iterations.Count; k++)
                {
                    if (iterations[k] < iterations[mindex])
                    {
                        mindex = k;
                    }
                }

                results.Add("");
                results[i] += "Lowest average iterations: " + iterations[mindex];
                results[i] += "\n\tAverage Time Expended: " + timings[mindex];
                results[i] += "\n\tMax Temperature: " + maxTemps[mindex/100];
                results[i] += "\n\tDecrement: " + decrements[mindex%100/10];
                results[i] += "\n\tThreshold: " + threshes[mindex%100%10];
                results[i] += "\n" + minState.toString("\t");
            }

            Console.WriteLine();
            Console.WriteLine("--------------------");
            Console.WriteLine("-----TESTS DONE-----");
            Console.WriteLine("--------------------");
            Console.WriteLine();

            for (int i = 0; i < runs; i++)
            {
                Console.WriteLine("\nTest " + (i+1) + " Results: ");
                Console.WriteLine(results[i]);
            }

            Console.WriteLine("The Original Graph that was generated: ");
            Console.WriteLine(board.toString(""));
        }
    }
}