using Newtonsoft.Json;

namespace Teloqui.Data {

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