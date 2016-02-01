using Newtonsoft.Json;

namespace Teloqui.Data.InlineMode {

	[JsonObject]
	public class ChosenInlineResult {

		[JsonProperty("result_id")]
		public string ResultId { get; set; }

		[JsonProperty("from")]
		public User From { get; set; }

		[JsonProperty("query")]
		public string Query { get; set; }
	}
}
