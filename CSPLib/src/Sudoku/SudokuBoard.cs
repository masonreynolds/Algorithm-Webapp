using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku_CSP
{
    public class SudokuBoard
    {
        public enum difficulty
        {
            Easy, 
            Medium, 
            Hard
        }

        public List<Cell[]> board { get; set; }


        public SudokuBoard(difficulty diff)
        {
            board = new List<Cell[]>();

            for (int i = 0; i < 9; i++)
            {
                board.Add(new Cell[9]);

                for (int k = 0; k < 9; k++)
                {
                    board[i][k] = new Cell();
                }
            }

            generateStartingBoard(diff);
        }

        public SudokuBoard(SudokuBoard b)
        {
            this.board = new List<Cell[]>();

            for (int i = 0; i < 9; i++)
            {
                board.Add(new Cell[9]);

                for (int k = 0; k < 9; k++)
                {
                    this.board[i][k] = new Cell(b.board[i][k]);
                }
            }
        }

        private void generateStartingBoard(difficulty diff)
        {
            this.board = CSP.startCSP(this).board.board;
            Random rand = new Random();
            int removed;

            if (diff == difficulty.Easy)
            {
                removed = 50;
            }
            else if (diff == difficulty.Medium)
            {
                removed = 60;
            }
            else
            {
                removed = 70;
            }

            for (int i = 0; i < removed; i++)
            {
                int row = rand.Next(9);
                int col = rand.Next(9);

                if (this.board[row][col].value != 0)
                {
                    this.board[row][col].value = 0;
                }
                else
                {
                    i--;
                }
            }

            for (int i = 0; i < 9; i++)
            {
                for (int k = 0; k < 9; k++)
                {
                    if (board[i][k].value == 0)
                    {
                        for (int val = 1; val < 10; val++)
                        {
                            if (!board[i][k].domain.Contains(val) && 
                                !Array.Exists(getRow(i), c => c.value == val) && 
                                !Array.Exists(getCol(k), c => c.value == val) &&
                                !Array.Exists(getSection(i, k), c => c.value == val))
                            {
                                board[i][k].addDomain(val);
                            }
                            else if (Array.Exists(getRow(i), c => c.value == val) && 
                                    Array.Exists(getCol(k), c => c.value == val) &&
                                    Array.Exists(getSection(i, k), c => c.value == val))
                            {
                                board[i][k].removeDomain(val);
                            }
                        }
                    }
                }
            }
        }

        public Cell[] getRow(int row)
        {
            Cell[] r = new Cell[9];

            for (int i = 0; i < 9; i++)
            {
                r[i] = board[row][i];
            }

            return r;
        }

        public Cell[] getCol(int col)
        {
            Cell[] c = new Cell[9];

            for (int i = 0; i < 9; i++)
            {
                c[i] = board[i][col];
            }

            return c;
        }

        public Cell[] getSection(int row, int col)
        {
            Cell[] s = new Cell[9];
            int sRow = (row / 3) * 3;
            int sCol = (col / 3) * 3;
            int count = 0;

            for (int i = sRow; i < sRow+3; i++)
            {
                for (int k = sCol; k < sCol+3; k++)
                {
                    s[count] = board[i][k];
                    count++;
                }
            }

            return s;
        }

        public void removeDomains(int val, int r, int c)
        {
            var section = getSection(r, c);
            var row = getRow(r);
            var col = getCol(c);

            for (int i = 0; i < 9; i++)
            {
                section[i].removeDomain(val);
                row[i].removeDomain(val);
                col[i].removeDomain(val);
            }
        }

        public void addDomains(int val, int r, int c)
        {
            var section = getSection(r, c);
            var row = getRow(r);
            var col = getCol(c);

            for (int i = 0; i < 9; i++)
            {
                section[i].addDomain(val);
                row[i].addDomain(val);
                col[i].addDomain(val);
            }
        }

        public int countFilled()
        {
            int filled = 0;

            for (int i = 0; i < 81; i++)
            {
                if (board[i/9][i%9].value != 0)
                {
                    filled++;
                }
            }

            return filled;
        }


        public string toString(string start)
        {
            string str = start;
            str += "Printing Board State: \n";
            str += start;

            for (int i = 0; i < 10 * 5 + 1; i++)
            {
                str += "-";
            }

            str += "\n" + start;

            for (int i = 0; i < 9; i++)
            {
                for (int k = 0; k < 9; k++)
                {
                    str += "[ ";
                    str += board[i][k].value;
                    str += " ]";

                    if (k % 3 == 2)
                    {
                        str += "   ";
                    }
                }

                if (i % 3 == 2 && i != 8)
                {
                    str += "\n" + start;;
                }

                str += "\n" + start;
            }

            for (int i = 0; i < 10 * 5 + 1; i++)
            {
                str += "-";
            }

            str += "\n";
            return str;
        }
    }
}