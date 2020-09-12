using System;

namespace Genetic_Algorithm
{
    public class BoardState
    {
        public int[] queens { get; set; }
        public int boardSize { get; set; }
        public int maxFitness { get; set; }
        public int fitness { get; set; }

        public BoardState(int boardSize)
        {
            queens = new int[boardSize];
            this.boardSize = boardSize;
            calculateMaxFitness();
            generateQueens();
            evaluateFitness();
        }

        public BoardState(BoardState board)
        {
            this.boardSize = board.boardSize;
            this.maxFitness = board.maxFitness;
            this.queens = new int[this.boardSize];
            Array.Copy(board.queens, this.queens, this.boardSize);
            this.fitness = board.fitness;
        }

        private void generateQueens()
        {
            Random rand = new Random();

            for (int i = 0; i < boardSize; i++)
            {
                queens[i] = rand.Next(boardSize);
            }
        }

        private void calculateMaxFitness()
        {
            int curr = boardSize - 1;
            this.maxFitness = 0;

            while (curr > 0)
            {
                maxFitness += curr--;
            }
        }

        public void evaluateFitness()
        {
            int attacking = 0;

            for (int i = 0; i < boardSize-1; i++)
            {
                for (int k = i+1; k < boardSize; k++)
                {
                    if (queens[i] == queens[k])
                    {
                        attacking++;
                    }
                    else if (Math.Abs(queens[i] - queens[k]) == Math.Abs(i - k))
                    {
                        attacking++;
                    }
                }
            }

            this.fitness = this.maxFitness - attacking;
        }

        private int getConflicts(int col)
        {
            int conflicts = 0;

            for (int i = 0; i < boardSize; i++)
            {
                if (i != col)
                {
                    if (queens[i] == queens[col])
                    {
                        conflicts++;
                    }
                    else if (Math.Abs(queens[i] - queens[col]) == Math.Abs(i -  col))
                    {
                        conflicts++;
                    }
                }
            }

            return conflicts;
        }

        private int[] getWorst(int num)
        {
            int[] worst = new int[num];

            for (int i = 0; i < boardSize; i++)
            {
                for (int k = 0; k < num; k++)
                {
                    int curr = getConflicts(i);

                    if (getConflicts(worst[k]) <= curr)
                    {
                        for (int j = k; j < num; j++)
                        {
                            worst[j] = curr;
                        }
                    }
                }
            }

            return worst;
        }

        public BoardState crossOver(BoardState other)
        {
            Random rand = new Random();
            int num = rand.Next(1, boardSize-1);
            int[] worst = getWorst(num);
            BoardState board = new BoardState(this);

            for (int i = 0; i < num; i++)
            {
                int move = rand.Next(boardSize);
                board.queens[worst[i]] = other.queens[move];
            }

            board.evaluateFitness();
            return board;
        }
    
        public void mutate()
        {
            Random rand = new Random();
            int change = rand.Next(0, boardSize);
            int queen = getWorst(1)[0];
            this.queens[queen] = change;
        }


        public string toString(string start)
        {
            string str = start;
            str += "Printing Board State with fitness of " + this.fitness + "\n";
            str += start;

            for (int i = 0; i < boardSize * 5; i++)
            {
                str += "-";
            }

            str += "\n" + start;

            for (int i = 0; i < boardSize; i++)
            {
                for (int k = 0; k < boardSize; k++)
                {
                    if (queens[i] == k)
                    {
                        str += "[ X ]";
                    }
                    else
                    {
                        str += "[ 0 ]";
                    }
                }

                str += "\n" + start;
            }

            for (int i = 0; i < boardSize * 5; i++)
            {
                str += "-";
            }

            str += "\n";
            return str;
        }
    }
}