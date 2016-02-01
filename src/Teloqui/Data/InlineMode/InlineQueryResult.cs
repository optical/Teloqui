using System;
using Newtonsoft.Json;

namespace Teloqui.Data.InlineMode {
	public abstract class InlineQueryResult {
		public const int MaxMessageTextLength = 512;

		private string _messageText;

		protected InlineQueryResult(string id) {
			Id = id;
		}

		[JsonProperty("type")]
		public abstract string Type { get; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("parse_mode")]
		// TODO: Make change type here from a string to an enum
		public string ParseMode { get; set; }

		[JsonProperty("disable_web_page_preview")]
		public bool DisableWebPagePreview { get; set; }

		[JsonProperty("thumb_url")]
		public Uri ThumbUrl { get; set; }

		[JsonProperty("message_text")]
		public string MessageText {
			get { return _messageText; }
			set {
				if (value.Length > MaxMessageTextLength) {
					throw new ArgumentException($"{nameof(MessageText)} may not be larger than {MaxMessageTextLength} in size");

				}
				_messageText = value;
			}
		}
	}
}
