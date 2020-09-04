using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku_CSP
{
    public class Cell
    {
        public List<int> domain { get; set; }
        public int value { get; set; }
        
        public Cell()
        {
            domain = new List<int>();

            for (int i = 1; i < 10; i++)
            {
                domain.Add(i);
            }
            
            value = 0;
        }

        public Cell(Cell other)
        {
            this.domain = new List<int>();

            for (int i = 0; i < other.domain.Count; i++)
            {
                this.domain.Add(other.domain[i]);
            }

            this.value = other.value;
        }

        public void removeDomain(int con)
        {
            domain.Remove(con);
        }

        public void addDomain(int con)
        {
            domain.Add(con);
        }

        public bool hasNoDomain()
        {
            return domain.Count == 0;
        }
    }
}