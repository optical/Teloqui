using Newtonsoft.Json;

namespace Teloqui.Data {

	[JsonObject]
	public class GroupChat : ChatSource { 

		[JsonProperty("title")]
		public string Title { get; set; }
	}
}