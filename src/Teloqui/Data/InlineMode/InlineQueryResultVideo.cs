using System;
using Newtonsoft.Json;

namespace Teloqui.Data.InlineMode {
	public class InlineQueryResultVideo : InlineQueryResult {
		public override string Type => "video";

		public InlineQueryResultVideo(string videoUri, string thumbUri, VideoMimeType mimeType, string messageText, string title)
			: this(new Uri(videoUri), new Uri(thumbUri), mimeType, messageText, title, Guid.NewGuid().ToString()) {
		}

		public InlineQueryResultVideo(Uri videoUri, Uri thumbUri, VideoMimeType mimeType, string messageText, string title) 
			: this(videoUri, thumbUri, mimeType, messageText, title, Guid.NewGuid().ToString()) {
		}

		public InlineQueryResultVideo(Uri vidoeIrUri, Uri thumbUri, VideoMimeType mimeType, string messageText, string title, string id) 
			: base(id) {
			VideoUrl = vidoeIrUri;
			ThumbUrl = thumbUri;
			MimeType = mimeType;
			MessageText = messageText;
			Title = title;
		}

		[JsonProperty("mime_type")]
		public VideoMimeType MimeType { get; set; }

		[JsonProperty("video_url")]
		public Uri VideoUrl { get; set; }

		[JsonProperty("video_width")]
		public int VideoWidth { get; set; }

		[JsonProperty("video_height")]
		public int VideoHeight { get; set; }

		[JsonProperty("video_duration")]
		public int VideoDuration { get; set; }
	}
}
