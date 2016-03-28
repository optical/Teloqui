using System;
using Newtonsoft.Json;

namespace Teloqui.Data.InlineMode {
	[JsonObject]
	public class InlineQueryResultGif : InlineQueryResultWithCaption {
		public override string Type => "gif";

		public InlineQueryResultGif(Uri gifUri, Uri thumbUri, string id)
			: base(id) {
			GifUrl = gifUri;
			ThumbUrl = thumbUri;
		}

		public InlineQueryResultGif(Uri gifUri, Uri thumbUri)
			: this(gifUri, thumbUri, Guid.NewGuid().ToString()) {
		}

		public InlineQueryResultGif(string gifUri, string tumbUri) 
			: this(new Uri(gifUri), new Uri(tumbUri)) {
		}

		[JsonProperty("gif_url")]
		public Uri GifUrl { get; set; }

		[JsonProperty("gif_width")]
		public int GifWidth { get; set; }

		[JsonProperty("gif_height")]
		public int GifHeight { get; set; }
	}
}
