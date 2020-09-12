using System;

namespace Graph
{
    public class Link
    {
        public Position start { get; set; }
        public Position end { get; set; }
        public double weight { get; set; }

        public Link(Position start, Position end)
        {
            this.start = start;
            this.end = end;
            calculateWeight();
        }

        public void calculateWeight()
        {
            this.weight = 1.609344 * 3963.0 * Math.Acos((Math.Sin(start.lat*Math.PI/180.0) * Math.Sin(end.lat*Math.PI/180.0)) + 
                                            Math.Cos(start.lat*Math.PI/180.0) * Math.Cos(end.lat*Math.PI/180.0) * 
                                            Math.Cos(end.lon*Math.PI/180.0 - start.lon*Math.PI/180.0));
        }
    }
}