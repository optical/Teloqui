using System;
using Newtonsoft.Json;

namespace Teloqui.Data.InlineMode {
	[JsonObject]
	public class InlineQueryResultGif : InlineQueryResultWithCaption {
		public override string Type => "gif";

		public InlineQueryResultGif(string id, Uri gifUri, Uri thumbUri) 
			: base(id) {
			GifUrl = gifUri;
			ThumbUrl = thumbUri;
		}

		[JsonProperty("gif_url")]
		public Uri GifUrl { get; set; }

		[JsonProperty("gif_width")]
		public int GifWidth { get; set; }

		[JsonProperty("gif_height")]
		public int GifHeight { get; set; }
	}
}
