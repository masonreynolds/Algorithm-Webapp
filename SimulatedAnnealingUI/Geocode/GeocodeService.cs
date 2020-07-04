using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;
using Graph;

namespace SimulatedAnnealingUI.Geocode
{
	public interface IGeocodeService
	{
		Task<Position> GetPos(string address);
	}

	public class GeocodeService : IGeocodeService
	{
		private HttpClient _http;
		private Random _rand;

		public GeocodeService(HttpClient http)
		{
			_http = http;
			_rand = new Random();
		}

		public async Task<Position> GetPos(string address)
		{
			var urlEncodedAddress = HttpUtility.UrlEncode(address);
			var response = await _http.GetFromJsonAsync<GeocodeResponse>($"https://api.opencagedata.com/geocode/v1/json?key=21af985845f14e3ba9b11335be4a2dae&q={urlEncodedAddress}");
			if (response.Results.Length != 0)
			{
				var position = new Position(_rand.Next(), response.Results[0].Geometry.Lat, response.Results[0].Geometry.Lng, address);
				return position;
			}
			else
			{
				return null;
			}
		}
	}
}
