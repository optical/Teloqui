using Newtonsoft.Json;

namespace Optigram.Data {

	[JsonObject]
	public class UserProfilePhotos {

		[JsonProperty("total_count")]
		public int TotalCount { get; set; }

		[JsonProperty("photos")]
		public PhotoSize[] Photos { get; set; }
	}
}