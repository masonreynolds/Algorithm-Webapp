using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph
{
	public class SimAnnealingGraph
	{
		public struct annealResult
		{
			public WeightedGraph state;
			public double iterations;
			public double temp;

			public annealResult(WeightedGraph state, double iterations, double temp)
			{
				this.state = state;
				this.iterations = iterations;
				this.temp = temp;
			}
		}

		private WeightedGraph start { get; set; }
		private double threshold { get; set; }
		private double decrement { get; set; }
		private int maxTemp { get; set; }
		public int iterations { get; set; }

		public static annealResult startSimAnneal(WeightedGraph start, int maxTemp, double decrement, double threshold)
		{
			var graph = new SimAnnealingGraph(start, maxTemp, decrement, threshold);

			annealResult solution = graph.runSimAnneal(start).ToList()[^1];

			return solution;
		}

		public static IEnumerable<annealResult> Run(WeightedGraph start, int maxTemp, double decrement, double threshold)
		{
			var graph = new SimAnnealingGraph(start, maxTemp, decrement, threshold);

			var solutions = graph.runSimAnneal(start);

			return solutions;
		}

		private SimAnnealingGraph(WeightedGraph start, int maxTemp, double decrement, double threshold)
		{
			this.start = start;
			this.threshold = threshold;
			this.decrement = decrement;
			this.maxTemp = maxTemp;
			this.iterations = 0;
		}

		private IEnumerable<annealResult> runSimAnneal(WeightedGraph start)
		{
			WeightedGraph currState = start;
			WeightedGraph lastState = null;
			double temp = maxTemp;

			while (temp >= threshold * maxTemp)
			{
				WeightedGraph nextState = currState.generateRandomNeighbor();
				double deltaEnergy = currState.distance - nextState.distance;
				Random rand = new Random();
				double r = rand.NextDouble();

				if (deltaEnergy >= 0)
				{
					currState = nextState;
				}
				else if (Math.Exp(deltaEnergy / temp) > r)
				{
					currState = nextState;
				}

				if (currState != lastState)
				{
					lastState = currState;
					yield return new annealResult(currState, iterations, temp);
				}

				temp *= decrement;
				iterations++;
			}

		}
	}
}
