using System.ComponentModel.DataAnnotations;
using System;

namespace Graph
{
	public class Position
	{
		static private int count = 0;

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
			this.name = "Custom Location " + (++count);
		}

		public Position(int ID, double lat, double lon)
		{
			this.name = "Custom Location " + (++count);
			this.lat = lat;
			this.lon = lon;
			this.ID = ID;
		}

		public Position(string name, double lat, double lon)
		{
			this.name = name;
			this.lat = lat;
			this.lon = lon;
		}

		public Position(int ID, string name, double lat, double lon)
		{
			this.ID = ID;
			this.lat = lat;
			this.lon = lon;
			this.name = name;
		}
	}
}