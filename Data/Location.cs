using Newtonsoft.Json;

namespace Optigram.Data {

	[JsonObject]
	public class Location {

		[JsonProperty("longitude")]
		public double Longitude { get; set; }

		[JsonProperty("latitude")]
		public double Lattitude { get; set; }
	}
}