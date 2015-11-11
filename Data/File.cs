using Newtonsoft.Json;

namespace Optigram.Data {
	[JsonObject]
	public class File {
		[JsonProperty("file_id")]
		public string Id { get; set; }

		[JsonProperty("file_size")]
		public int? Size { get; set; }

		[JsonProperty("file_path")]
		public string Path { get; set; }
	}
}