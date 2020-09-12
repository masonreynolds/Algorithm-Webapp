using System;
using System.Collections.Generic;
using System.Linq;

namespace SimAnnealing_NQueens
{
    public class SimAnnealingNQueens
    {
        public struct simBoard
        {
            public BoardState board;
            public double temp;
            public int iter;

            public simBoard(BoardState board, double temp, int iter)
            {
                this.board = board;
                this.temp = temp;
                this.iter = iter;
            }
        }

        private BoardState start { get; set; }
        private double threshold { get; set; }
        private double decrement { get; set; }
        private int maxTemp { get; set; }

        public static simBoard startSimAnneal(BoardState start, int maxTemp, double decrement, double threshold)
        {
            SimAnnealingNQueens sim = new SimAnnealingNQueens(start, maxTemp, decrement, threshold);

            simBoard solution = sim.runSimAnneal(start).ToList()[^1];

            return solution;
        }

		public static IEnumerable<simBoard> run(BoardState start, int maxTemp, double decrement, double threshold)
		{
			SimAnnealingNQueens sim = new SimAnnealingNQueens(start, maxTemp, decrement, threshold);

            var solution = sim.runSimAnneal(start);

            return solution;
		}

        public SimAnnealingNQueens(BoardState start, int maxTemp, double decrement, double threshold)
        {
            this.threshold = threshold;
            this.decrement = decrement;
            this.maxTemp = maxTemp;
            this.start = start;
        }

        private IEnumerable<simBoard> runSimAnneal(BoardState start)
        {
            BoardState currState = start;
            double temp = maxTemp;
            int iteration = 0;

            yield return new simBoard(currState, temp, iteration);

            while (temp >= threshold && currState.heuristic != 0)
            {
                BoardState nextState = currState.generateRandomNeighbor();
                int deltaEnergy = currState.heuristic - nextState.heuristic;
                Random rand = new Random();
                iteration++;

                if (deltaEnergy >= 0)
                {
                    currState = nextState;
                    yield return new simBoard(currState, temp, iteration);
                }
                else if (Math.Exp(((double)deltaEnergy) / temp) < rand.NextDouble())
                {
                    currState = nextState;
                    yield return new simBoard(currState, temp, iteration);
                }

                temp *= decrement;
            }
        }
    }
}
