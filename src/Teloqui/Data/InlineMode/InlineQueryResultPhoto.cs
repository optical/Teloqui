using System;
using Newtonsoft.Json;

namespace Teloqui.Data.InlineMode {
	[JsonObject]
	public class InlineQueryResultPhoto : InlineQueryResult {

		public override string Type => "photo";

		public InlineQueryResultPhoto(string id, Uri photoUri, Uri thumbUri)
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
