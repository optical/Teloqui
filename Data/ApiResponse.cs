using Newtonsoft.Json;

namespace Optigram.Data {

	[JsonObject]
	public class ApiResponse<T> {

		[JsonProperty("ok")]
		public bool Ok { get; set; }

		[JsonProperty("result")]
		public T Result { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("error_code")]
		public int? ErrorCode { get; set; }
	}
}