using System;
using Newtonsoft.Json;

namespace Teloqui.Data.InlineMode {
	public abstract class InlineQueryResultWithCaption : InlineQueryResult {
		public const int MaxCaptionLength = 200;

		private string _caption;

		protected InlineQueryResultWithCaption(string id) : base(id) {
		}

		[JsonProperty("caption")]
		public string Caption {
			get { return _caption; }
			set {
				if (value.Length > MaxCaptionLength) {
					throw new ArgumentException($"{nameof(Caption)} may not be larger than {MaxCaptionLength} in size!");
				}

				_caption = value;
			}
		}
	}
}
