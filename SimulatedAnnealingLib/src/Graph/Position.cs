using System.ComponentModel.DataAnnotations;
using System;

namespace Graph
{
	public class Position
	{
		static private char currName = 'A';

		[Required]
		public int ID { get; set; }
		[Required]
		[Range(-90, 90)]
		public double lat { get; set; }
		[Required]
		[Range(-180, 180)]
		public double lon { get; set; }
		[Required]
		public string name { get; set; }

		public Position()
		{
			this.name = currName.ToString();
			currName++;
		}

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

		public Position(int iD, double lat, double lon, string name)
		{
			ID = iD;
			this.lat = lat;
			this.lon = lon;
			this.name = name;
		}
	}
}