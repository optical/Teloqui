using Newtonsoft.Json;

namespace Teloqui.Data.InlineMode {
	[JsonObject]
	public class InlineQueryResultArticle : InlineQueryResult {
		public override string Type => "article";

		public InlineQueryResultArticle(string id, string title, string message)
			: base(id) {
			Title = title;
			MessageText = message;
		}

		[JsonProperty("hide_url")]
		public bool HideUrl { get; set; }


		[JsonProperty("thumb_width")]
		public int ThumbnailWidth { get; set; }

		[JsonProperty("thumb_height")]
		public int ThumbnailHeight { get; set; }
	}
}
