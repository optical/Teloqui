using Newtonsoft.Json;

namespace Teloqui.Data {

	public abstract class ChatSource {

		[JsonProperty("id")]
		public int Id { get; set; }
	}
}