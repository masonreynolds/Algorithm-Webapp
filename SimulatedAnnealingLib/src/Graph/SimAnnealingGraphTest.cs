using System;
using System.Collections.Generic;

namespace Graph
{
	static class SimAnnealingGraphTest
	{
		private static int[] maxTemps = new int[8] { 1, 10, 100, 1000, 10000, 100000, 1000000, 10000000 };
		private static double[] decrements = new double[10] { 0.91, 0.92, 0.93, 0.94, 0.95, 0.96, 0.97, 0.98, 0.99, 0.999 };
		private static double[] threshes = new double[10] { 0.5, 0.4, 0.3, 0.25, 0.2, 0.15, 0.1, 0.05, 0.01, 0.001 };
		private static List<double> distances;
		private static List<int> iterations;
		private static List<string> results;

		public static void RunTests(int numPoses, int runs)
		{
			WeightedGraph start = new WeightedGraph(numPoses);
			results = new List<string>();
			WeightedGraph minState = start;

			for (int i = 0; i < runs; i++)
			{
				Console.WriteLine();
				Console.WriteLine("----------------------------");
				Console.WriteLine("----- TESTS " + (i + 1) + " STARTING -----");
				Console.WriteLine("----------------------------");
				Console.WriteLine();

				distances = new List<double>();
				iterations = new List<int>();

				foreach (var temp in maxTemps)
				{
					Console.WriteLine("Running Tests with Max Temperature of " + temp + ": ");

					foreach (var dec in decrements)
					{
						Console.WriteLine("\tRunning Tests with decrement of " + dec + ": ");

						foreach (var thresh in threshes)
						{
							Console.WriteLine("\t\tRunning Tests with threshold of " + thresh + ": ");
							WeightedGraph finalState = new WeightedGraph(numPoses);
							double avDist = 0;
							int avIter = 0;

							for (int k = 0; k < 10; k++)
							{
								var currentIterations = 0;
								(finalState, currentIterations) = SimAnnealingGraph.startSimAnneal(start, temp, dec, thresh);

								if (finalState.distance < minState.distance)
								{
									minState = finalState;
								}

								avIter += currentIterations;
								avDist += finalState.distance;
							}

							iterations.Add(avIter / 10);
							distances.Add(avDist / 10);
						}
					}
				}

				int mindex = 0;

				for (int k = 1; k < distances.Count; k++)
				{
					if (distances[k] < distances[mindex])
					{
						mindex = k;
					}
					else if (distances[k] == distances[mindex])
					{
						if (iterations[k] < iterations[mindex])
						{
							mindex = k;
						}
					}
				}

				results.Add("");
				results[i] += "\tLowest average distance: " + Math.Round(distances[mindex], 2);
				results[i] += "\n\tAverage Max Iterations: " + iterations[mindex];
				results[i] += "\n\tMax Temperature: " + maxTemps[mindex / 100];
				results[i] += "\n\tDecrement: " + decrements[mindex % 100 / 10];
				results[i] += "\n\tThreshold: " + threshes[mindex % 100 % 10];
				results[i] += "\n" + minState.toString("\t");
			}

			Console.WriteLine();
			Console.WriteLine("--------------------");
			Console.WriteLine("-----TESTS DONE-----");
			Console.WriteLine("--------------------");
			Console.WriteLine();

			for (int i = 0; i < runs; i++)
			{
				Console.WriteLine("\nTest " + (i + 1) + " Results: ");
				Console.WriteLine(results[i]);
			}

			Console.WriteLine("The Original WeightedGraph that was generated: ");
			Console.WriteLine(start.toString(""));
		}
	}
}
