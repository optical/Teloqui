using System;
using Newtonsoft.Json;

namespace Teloqui.Data.InlineMode {
	[JsonObject]
	public class InlineQueryResultPhoto : InlineQueryResult {

		public override string Type => "photo";

		public InlineQueryResultPhoto(string photoUri)
			: this(new Uri(photoUri)) {
		}

		public InlineQueryResultPhoto(string photoUri, string id)
			: this(new Uri(photoUri), id) {
		}

		public InlineQueryResultPhoto(Uri photoUri) 
			: this(photoUri, Guid.NewGuid().ToString()) {
		}

		public InlineQueryResultPhoto(Uri photoUri, string id)
			: base(id) {
			PhotoUrl = photoUri;
			// Non-optional argument, but can be overridden with a proper thumbnail as needed
			ThumbUrl = PhotoUrl;
		}

		[JsonProperty("photo_url")]
		public Uri PhotoUrl { get; set; }

		[JsonProperty("photo_width")]
		public int PhotoWidth { get; set; }

		[JsonProperty("photo_height")]
		public int PhotoHeight { get; set; }


	}
}
