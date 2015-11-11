using Newtonsoft.Json;

namespace Optigram.Data {

	public abstract class ChatSource {

		[JsonProperty("id")]
		public int Id { get; set; }
	}
}