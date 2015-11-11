using Newtonsoft.Json;

namespace Optigram.Data {

	[JsonObject]
	public class Update {

		[JsonProperty("update_id")]
		public int UpdateId { get; set; }

		[JsonProperty("message")]
		public Message Message { get; set; }
	}
}