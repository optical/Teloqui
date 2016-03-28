using System;
using Newtonsoft.Json;

namespace Teloqui.Data.InlineMode {
	[JsonObject]
	public class InlineQueryResultPhoto : InlineQueryResult {

		public override string Type => "photo";

		public InlineQueryResultPhoto(Uri photoUri, Uri thumbUri) 
			: this(photoUri, thumbUri, Guid.NewGuid().ToString()) {
		}

		public InlineQueryResultPhoto(Uri photoUri, Uri thumbUri, string id)
			: base(id) {
			PhotoUrl = photoUri;
			ThumbUrl = thumbUri;
		}

		[JsonProperty("photo_url")]
		public Uri PhotoUrl { get; set; }

		[JsonProperty("photo_width")]
		public int PhotoWidth { get; set; }

		[JsonProperty("photo_height")]
		public int PhotoHeight { get; set; }


	}
}
