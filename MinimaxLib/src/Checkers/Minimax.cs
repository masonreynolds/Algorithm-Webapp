using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkers_Minimax
{
    public class Minimax
    {
        public struct minimaxResult
        {
            public CheckerBoard board;
            public int states;

            public minimaxResult(CheckerBoard board, int states)
            {
                this.board = board;
                this.states = states;
            }
        }

        private CheckerBoard start { get; set; }
        private int states { get; set; }

        public static minimaxResult startMinimax(CheckerBoard start, int depth)
        {
            start.redTurn = false;
            Minimax minimax = new Minimax(start);

            minimaxResult solution = minimax.setupMinimax(start, depth);

            return solution;
        }

        public Minimax(CheckerBoard start)
        {
            this.start = start;
        }

        private minimaxResult setupMinimax(CheckerBoard start, int depth)
        {
            states = 0;

            if (Math.Abs(start.heuristic) == 100 || depth == 0)
            {
                return new minimaxResult(start, states);
            }

            var neighbors = start.generateNeighbors();
            CheckerBoard bestMove = start;

            if (start.redTurn)
            {
                int best = int.MaxValue;

                foreach (var neighbor in neighbors)
                {
                    int result = runMinimax(neighbor, best, depth-1);

                    if (result < best)
                    {
                        bestMove = neighbor;
                        best = result;
                    }
                }

                if (neighbors.Count == 0)
                {
                    best = 100;
                }
            }
            else
            {
                int best = int.MinValue;

                foreach (var neighbor in neighbors)
                {
                    int result = runMinimax(neighbor, best, depth-1);

                    if (result > best)
                    {
                        bestMove = neighbor;
                        best = result;
                    }
                }

                if (neighbors.Count == 0)
                {
                    best = -100;
                }
            }
            

            return new minimaxResult(bestMove, states);
        }

        private int runMinimax(CheckerBoard current, int AB, int depth)
        {
            states++;

            if (Math.Abs(current.heuristic) == 100 || depth == 0)
            {
                return current.heuristic;
            }

            var neighbors = current.generateNeighbors();

            if (current.redTurn)
            {
                int beta = int.MaxValue;

                foreach (var neighbor in neighbors)
                {
                    int result = runMinimax(neighbor, beta, depth-1);

                    if (result < beta)
                    {
                        beta = result;
                    }

                    if (beta < AB)
                    {
                        break;
                    }
                }

                if (neighbors.Count == 0)
                {
                    beta = 100;
                }

                return beta;
            }
            else
            {
                int alpha = int.MinValue;

                foreach (var neighbor in neighbors)
                {
                    int result = runMinimax(neighbor, alpha, depth-1);

                    if (result > alpha)
                    {
                        alpha = result;
                    }

                    if (alpha > AB)
                    {
                        break;
                    }
                }

                if (neighbors.Count == 0)
                {
                    alpha = -100;
                }

                return alpha;
            }
        }
    }
}