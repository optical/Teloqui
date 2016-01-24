using Newtonsoft.Json;

namespace Teloqui.Data {

	[JsonObject]
	public class Location {

		[JsonProperty("longitude")]
		public double Longitude { get; set; }

		[JsonProperty("latitude")]
		public double Lattitude { get; set; }
	}
}