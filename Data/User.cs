using Newtonsoft.Json;

namespace Optigram.Data {

	[JsonObject]
	public class User : ChatSource {

		[JsonProperty("first_name")]
		public string FirstName { get; set; }

		[JsonProperty("last_name")]
		public string LastName { get; set; }

		[JsonProperty("username")]
		public string Username { get; set; }
	}
}