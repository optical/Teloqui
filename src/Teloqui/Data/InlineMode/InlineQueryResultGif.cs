using System;
using Newtonsoft.Json;

namespace Teloqui.Data.InlineMode {
	[JsonObject]
	public class InlineQueryResultGif : InlineQueryResultWithCaption {
		public override string Type => "gif";

		public InlineQueryResultGif(Uri gifUri, string id)
			: base(id) {
			GifUrl = gifUri;
			// Non-optional argument, but can be overridden with a proper thumbnail as needed
			ThumbUrl = GifUrl;
		}

		public InlineQueryResultGif(Uri gifUri)
			: this(gifUri, Guid.NewGuid().ToString()) {
		}

		public InlineQueryResultGif(string gifUri, string id)
			: this(new Uri(gifUri), id) {
		}

		public InlineQueryResultGif(string gifUri)
			: this(new Uri(gifUri)) {
		}

		[JsonProperty("gif_url")]
		public Uri GifUrl { get; set; }

		[JsonProperty("gif_width")]
		public int GifWidth { get; set; }

		[JsonProperty("gif_height")]
		public int GifHeight { get; set; }
	}
}
