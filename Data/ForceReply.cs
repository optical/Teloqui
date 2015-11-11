using Newtonsoft.Json;

namespace Optigram.Data {

	[JsonObject]
	public class ForceReply {

		[JsonProperty("force_reply")]
		public bool ShouldForceReply {
			get { return true; }
		}

		[JsonProperty("selective")]
		public bool Selective { get; set; }
	}
}