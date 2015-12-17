using Newtonsoft.Json;

namespace Teloqui.Data {

	[JsonObject]
	public class ReplyKeyboardHide {

		[JsonProperty("hide_keyboard")]
		public bool HideKeyboard => true;

		[JsonProperty("selective")]
		public bool Selective { get; set; }
	}
}