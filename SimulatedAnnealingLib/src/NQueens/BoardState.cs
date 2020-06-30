using System;

namespace NQueens
{
    public class BoardState
    {
        private int[] queens { get; set; }
        private int boardSize { get; set; }
        public int heuristic { get; set; }

        public BoardState(int boardSize)
        {
            queens = new int[boardSize];
            this.boardSize = boardSize;
            generateQueens();
            evaluateHeuristic();
        }

        public BoardState(BoardState board)
        {
            this.boardSize = board.boardSize;
            this.queens = new int[this.boardSize];
            Array.Copy(board.queens, this.queens, this.boardSize);
            this.heuristic = board.heuristic;
        }

        private void generateQueens()
        {
            Random rand = new Random();

            for (int i = 0; i < boardSize; i++)
            {
                queens[i] = rand.Next(boardSize);
            }
        }

        private void evaluateHeuristic()
        {
            int heuristic = 0;

            for (int i = 0; i < boardSize-1; i++)
            {
                for (int k = i+1; k < boardSize; k++)
                {
                    if (queens[i] == queens[k])
                    {
                        heuristic++;
                    }

                    if (Math.Abs(queens[i] - queens[k]) == Math.Abs(i - k))
                    {
                        heuristic++;
                    }
                }
            }

            this.heuristic = heuristic;
        }

        public BoardState generateRandomNeighbor()
        {
            BoardState newBoard = new BoardState(this);
            Random rand = new Random();

            int queen = rand.Next(newBoard.boardSize);
            int pos = newBoard.queens[queen];
            while(pos == newBoard.queens[queen]) { pos = rand.Next(0, newBoard.boardSize); }
            newBoard.queens[queen] = pos;

            newBoard.evaluateHeuristic();
            return newBoard;
        }
    
        public string toString(string start)
        {
            string str = start;
            str += "Printing Board State with Heuristic of " + this.heuristic + "\n";
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