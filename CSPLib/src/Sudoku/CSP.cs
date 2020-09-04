using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku_CSP
{
    public class CSP
    {
        public struct CSPResult
        {
            public SudokuBoard board;
            public int attempts;

            public CSPResult(SudokuBoard board, int attempts)
            {
                this.board = board;
                this.attempts = attempts;
            }
        }

        private SudokuBoard start { get; set; }
        private int backtracks { get; set; }

        public static CSPResult startCSP(SudokuBoard start)
        {
            CSP csp = new CSP(start);

            CSPResult solution = csp.runCSP(start, 0, 0).ToList()[^1];

            return solution;
        }

        public static IEnumerable<CSPResult> run(SudokuBoard start)
		{
			CSP sim = new CSP(start);

            var solution = sim.runCSP(start, 0, 0);

            return solution;
		}

        public CSP(SudokuBoard start)
        {
            this.start = start;
        }

        private IEnumerable<CSPResult> runCSP(SudokuBoard start, int row, int col)
        {
            List<SudokuBoard> moves = new List<SudokuBoard>();
            Random rand = new Random();

            moves.Add(start);
            moves.Add(new SudokuBoard(start));
            int filled = start.countFilled();
            
            while ((moves.Count - 2) < (81 - filled))
            {
                (int currRow, int currCol) = getLowestDomain(moves[^1]);

                if (!moves[^1].board[currRow][currCol].hasNoDomain())
                {
                    int index = rand.Next(moves[^1].board[currRow][currCol].domain.Count);

                    moves[^1].board[currRow][currCol].value = moves[^1].board[currRow][currCol].domain[index];
                    moves[^1].board[currRow][currCol].removeDomain(moves[^1].board[currRow][currCol].value);

                    var current = new SudokuBoard(moves[^1]);
                    moves.Add(current);

                    moves[^1].removeDomains(moves[^1].board[currRow][currCol].value, currRow, currCol);
                }
                else
                {
                    moves.Remove(moves[^1]);
                    (currRow, currCol) = getLastMove(moves[^1], moves[^2]);
                    moves[^1].board[currRow][currCol].value = 0;
                    backtracks++;
                }
                
                yield return new CSPResult(moves[^1], backtracks);
            }
        }

        private (int row, int col) getLowestDomain(SudokuBoard board)
        {
            int minRow = 0;
            int minCol = 0;

            for (int i = 1; i < 81; i++)
            {
                if (board.board[i/9][i%9].value == 0)
                {
                    if (board.board[minRow][minCol].value != 0 || board.board[i/9][i%9].domain.Count < board.board[minRow][minCol].domain.Count)
                    {
                        minRow = i / 9;
                        minCol = i % 9;
                    }
                }
            }

            return (minRow, minCol);
        }
    
        private (int row, int col) getLastMove(SudokuBoard current, SudokuBoard last)
        {
            int lastRow = 0;
            int lastCol = 0;

            for (int i = 0; i < 81; i++)
            {
                if (current.board[i/9][i%9].value != last.board[i/9][i%9].value)
                {
                    lastRow = i / 9;
                    lastCol = i % 9;
                    break;
                }
            }

            return (lastRow, lastCol);
        }
    }
}