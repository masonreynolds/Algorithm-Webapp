using System;

namespace Graph
{
    public class Position
    {
        static private char currName = 'A';
        public double lat { get; set; }
        public double lon { get; set; }
        public string name { get; set; }

        public Position(double lat, double lon)
        {
            this.name = currName.ToString();
            this.lat = lat;
            this.lon = lon;
            currName++;
        }

        public Position(string name, double lat, double lon)
        {
            this.name = name;
            this.lat = lat;
            this.lon = lon;
        }
    }
}