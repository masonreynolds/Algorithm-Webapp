using System;

namespace Graph
{
    static class SimAnnealingGraph
    {
        private static WeightedGraph start { get; set; }
        private static double threshold { get; set; }
        private static double decrement { get; set; }
        private static int maxTemp { get; set; }
        public static int iterations { get; set; }

        public static WeightedGraph startSimAnneal(WeightedGraph start, int maxTemp, double  decrement, double threshold)
        {
            SimAnnealingGraph.threshold = threshold;
            SimAnnealingGraph.decrement = decrement;
            SimAnnealingGraph.maxTemp = maxTemp;
            SimAnnealingGraph.start = start;
            SimAnnealingGraph.iterations = 0;

            WeightedGraph solution = runSimAnneal(start);

            return solution;
        }

        private static WeightedGraph runSimAnneal(WeightedGraph start)
        {
            WeightedGraph currState = start;
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

                temp *= decrement;
                iterations++;
            }

            return currState;
        }
    }
}
