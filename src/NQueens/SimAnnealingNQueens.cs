using System;

namespace NQueens
{
    static class SimAnnealingNQueens
    {
        private static BoardState start { get; set; }
        private static double threshold { get; set; }
        private static double decrement { get; set; }
        public static double seconds { get; set; }
        public static int iteration { get; set; }
        private static int maxTemp { get; set; }

        public static BoardState startSimAnneal(BoardState start, int maxTemp, double  decrement, double threshold)
        {
            SimAnnealingNQueens.threshold = threshold;
            SimAnnealingNQueens.decrement = decrement;
            SimAnnealingNQueens.maxTemp = maxTemp;
            SimAnnealingNQueens.start = start;

            BoardState solution = runSimAnneal(start);

            return solution;
        }

        private static BoardState runSimAnneal(BoardState start)
        {
            BoardState currState = start;
            double temp = maxTemp;
            iteration = 0;

            DateTime begin = DateTime.UtcNow;

            while (temp >= threshold * maxTemp && currState.heuristic != 0)
            {
                BoardState nextState = currState.generateRandomNeighbor();
                int deltaEnergy = currState.heuristic - nextState.heuristic;
                Random rand = new Random();
                iteration++;

                if (deltaEnergy >= 0)
                {
                    currState = nextState;
                }
                else if (Math.Exp(((double)deltaEnergy) / temp) < rand.NextDouble())
                {
                    currState = nextState;
                }

                temp *= decrement;
            }

            if (currState.heuristic != 0)
            {
                iteration = int.MaxValue;
            }

            TimeSpan diff = DateTime.UtcNow - begin;
            seconds = diff.TotalSeconds;

            return currState;
        }
    }
}
