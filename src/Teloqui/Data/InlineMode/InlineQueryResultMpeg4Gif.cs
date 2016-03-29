using System;
using Newtonsoft.Json;

namespace Teloqui.Data.InlineMode {
	public class InlineQueryResultMpeg4Gif : InlineQueryResultWithCaption {

		public override string Type => "mpeg4_gif";

		
		public InlineQueryResultMpeg4Gif(string mpeg4Uri, string thumbUri)
			: this(new Uri(mpeg4Uri), new Uri(thumbUri)) {
		}

		public InlineQueryResultMpeg4Gif(string mpeg4Uri, string thumbUri, string id)
			: this(new Uri(mpeg4Uri), new Uri(thumbUri), id) {
		}

		public InlineQueryResultMpeg4Gif(Uri mpeg4Uri, Uri thumbUri)
		: this(mpeg4Uri, thumbUri, Guid.NewGuid().ToString()) {

		}

		public InlineQueryResultMpeg4Gif(Uri mpeg4Uri, Uri thumbUri, string id)
			: base(id) {
			Mpeg4Url = mpeg4Uri;
			ThumbUrl = thumbUri;
		}

		[JsonProperty("mpeg4_url")]
		public Uri Mpeg4Url { get; set; }

		[JsonProperty("mpeg4_width")]
		public int Mpeg4Width { get; set; }

		[JsonProperty("mpeg4_height")]
		public int Mpeg4Height { get; set; }


	}
}
