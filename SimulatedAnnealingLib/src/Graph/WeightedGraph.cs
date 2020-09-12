using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph
{
    public class WeightedGraph
    {
        public List<Position> positions { get; set; }
        public List<Link> links { get; set; }
        public double distance { get; set; }

        public WeightedGraph(int numPoses)
        {
            positions = new List<Position>();
            links = new List<Link>();
            generatePositions(numPoses);
            generateLinks();
        }

        public WeightedGraph(List<Position> positions)
        {
            this.positions = positions;
            links = new List<Link>();
            generateLinks();
        }
        
        public WeightedGraph(WeightedGraph WeightedGraph)
        {
            this.positions = WeightedGraph.positions;
            this.links = new List<Link>();
            
            foreach (var link in WeightedGraph.links)
            {
                this.links.Add(new Link(link.start, link.end));
            }

            this.distance = WeightedGraph.distance;
        }

        private void generatePositions(int numPoses)
        {
            Random rand = new Random();

            for (int i = 0; i < numPoses; i++)
            {
                this.positions.Add(new Position(rand.Next(100000), rand.NextDouble() * 180 - 90, rand.NextDouble() * 360 - 180));
            }
        }

        private void generateLinks()
        {
            Random rand = new Random();
            int current = 0;

            while (links.Count < positions.Count)
            {
                int index;

                do
                {
                    index = rand.Next(positions.Count);

                    if (links.Count == positions.Count - 1)
                    {
                        index = 0;
                        break;
                    }
                }
                while (index == current || links.Any(link => link.start == positions[index]));

                Link link = new Link(positions[current], positions[index]);
                links.Add(link);
                current = index;
            }

            evaluateDistance();
        }

        private void evaluateDistance()
        {
            double distance = 0;

            foreach (var link in links)
            {
                distance += link.weight;
            }

            this.distance = distance;
        }

        public WeightedGraph generateRandomNeighbor()
        {
            WeightedGraph neighbor = new WeightedGraph(this);
            Random rand = new Random();
            Position pos1;
            Position pos2;

            do 
            {
                pos1 = positions[rand.Next(positions.Count)];
                pos2 = positions[rand.Next(positions.Count)];
            }
            while (pos2 == pos1);

            for (int i = 0; i < neighbor.links.Count; i++)
            {
                if (neighbor.links[i].start == pos1)
                {
                    neighbor.links[i].start = pos2;
                }
                else if (neighbor.links[i].start == pos2)
                {
                    neighbor.links[i].start = pos1;
                }

                if (neighbor.links[i].end == pos1)
                {
                    neighbor.links[i].end = pos2;
                }
                else if (neighbor.links[i].end == pos2)
                {
                    neighbor.links[i].end = pos1;
                }
                
                neighbor.links[i].calculateWeight();
            }

            neighbor.evaluateDistance();

            return neighbor;
        }

        public string toString(string start)
        {
            string str = start;

           str += "Printing WeightedGraph with Total Distance of " + Math.Round(this.distance, 2) + "\n";
           str += start;

            for (int i = 0; i < 20; i++)
            {
                str += "-";
            }

            str += "\n";

            foreach (var link in links)
            {
                str += start + link.start.name + " --- " + Math.Round(link.weight, 2) + " --> " + link.end.name + "\n";
            }

            str += start;
            
            for (int i = 0; i < 20; i++)
            {
                str += "-";
            }

            str += "\n";

            return str;
        }
    }
}