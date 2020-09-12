using System;
using System.Collections.Generic;
using System.Linq;

namespace Genetic_Algorithm
{
	public class GeneticAlgorithm
	{
        public struct genResult 
        {
            public List<BoardState> pop;
            public int generation;

            public genResult(List<BoardState> pop, int generation)
            {
                this.pop = pop;
                this.generation = generation;
            }
        }

        List<BoardState> population;
        int totalFitness;

        public static genResult startGenAlgorithm(int boardSize, int popSize)
		{
			GeneticAlgorithm gen = new GeneticAlgorithm(boardSize, popSize);

            genResult solution = gen.runGenAlgorithm().ToList()[^1];

			return solution;
		}

		public static IEnumerable<genResult> run(int boardSize, int popSize)
		{
			GeneticAlgorithm gen = new GeneticAlgorithm(boardSize, popSize);

			var solutions = gen.runGenAlgorithm();

			return solutions;
		}

		private GeneticAlgorithm(int boardSize, int popSize)
		{
            population = new List<BoardState>();

            for (int i = 0; i < popSize; i++)
            {
                population.Add(new BoardState(boardSize));
            }

            evaluateTotalFitness();
		}

        private void evaluateTotalFitness()
        {
            this.totalFitness = 0;

			for (int i = 0; i < population.Count; i++)
            {
                this.totalFitness += population[i].fitness;
            }
        }

        private List<BoardState[]> makeSelections()
        {
            List<BoardState[]> pairs = new List<BoardState[]>();
            Random rand = new Random();

            for (int i = 0; i < population.Count; i++)
            {
                BoardState parent1;
                BoardState parent2;

                do 
                {
                    int choice1 = rand.Next(1, this.totalFitness+1);
                    int choice2 = rand.Next(1, this.totalFitness+1);
                    parent1 = null;
                    parent2 = null;
                    int sum = 0;

                    for (int k = 0; k < population.Count; k++)
                    {
                        sum += population[k].fitness;

                        if (choice1 <= sum && parent1 == null)
                        {
                            parent1 = population[k];
                        }

                        if (choice2 <= sum && parent2 == null)
                        {
                            parent2 = population[k];
                        }

                        if (parent1 != null && parent2 != null)
                        {
                            break;
                        }
                    }
                }
                while (parent1 == parent2);

                pairs.Add(new BoardState[] {parent1, parent2});
            }

            return pairs;
        }

        private void makeCrossovers(List<BoardState[]> pairs)
        {
            population.Clear();

            foreach (var pair in pairs)
            {
                population.Add(pair[0].crossOver(pair[1]));
            }

            evaluateTotalFitness();
        }

        private void makeMutations()
        {
            for (int i = 0; i < population.Count; i++)
            {
                Random rand = new Random();

                if (rand.Next(100) < 20)
                {
                    population[i].mutate();
                }
            }
        }

        private BoardState getSolution()
        {
            BoardState solution = null;

            foreach (var state in population)
            {
                if (state.fitness == state.maxFitness)
                {
                    solution = state;
                    break;
                }
            }

            return solution;
        }

		private IEnumerable<genResult> runGenAlgorithm()
		{
            int generation = 1;

            do
            {
                yield return new genResult(this.population, generation++);
			    List<BoardState[]> pairs = this.makeSelections();
                makeCrossovers(pairs);
                makeMutations();
            }
            while (this.getSolution() == null);

            yield return new genResult(this.population, generation);
		}
	}
}
