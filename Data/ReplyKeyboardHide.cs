using Newtonsoft.Json;

namespace Optigram.Data {

	[JsonObject]
	public class ReplyKeyboardHide {

		[JsonProperty("hide_keyboard")]
		public bool HideKeyboard {
			get { return true; }
		}

		[JsonProperty("selective")]
		public bool Selective { get; set; }
	}
}