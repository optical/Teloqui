using System;
using Newtonsoft.Json;

namespace Teloqui.Data.InlineMode {
	[JsonObject]
	public class InlineQueryResultArticle : InlineQueryResult {
		public override string Type => "article";

		public InlineQueryResultArticle(string title, string message) 
			: this(title, message, Guid.NewGuid().ToString()) {
		}

		public InlineQueryResultArticle(string title, string message, string id)
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
