using Newtonsoft.Json;

namespace Optigram.Data {

	[JsonObject]
	public class GroupChat : ChatSource { 

		[JsonProperty("title")]
		public string Title { get; set; }
	}
}