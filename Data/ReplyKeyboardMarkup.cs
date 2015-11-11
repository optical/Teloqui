using Newtonsoft.Json;

namespace Optigram.Data {

	[JsonObject]
	public class ReplyKeyboardMarkup {

		[JsonProperty("keyboard")]
		public string[][] Keyboard { get; set; }

		[JsonProperty("resize_keyboard")]
		public bool ResizeKeyboard { get; set; }

		[JsonProperty("one_time_keyboard")]
		public bool OneTimeKeyboard { get; set; }

		[JsonProperty("selective")]
		public bool Selective { get; set; }
	}
}