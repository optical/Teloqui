using Newtonsoft.Json;
using Teloqui.Data.InlineMode;

namespace Teloqui.Data {

	[JsonObject]
	public class Update {

		[JsonProperty("update_id")]
		public int UpdateId { get; set; }

		[JsonProperty("message")]
		public Message Message { get; set; }

		[JsonProperty("inline_query")] 
		public InlineQuery InlineQuery { get; set; }

		[JsonProperty("chosen_inline_result")]
		public ChosenInlineResult ChosenInlineResult { get; set; }
	}
}