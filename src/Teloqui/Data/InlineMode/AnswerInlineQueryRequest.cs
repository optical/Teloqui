using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Teloqui.Data.InlineMode {
	[JsonObject]
	public class AnswerInlineQueryRequest {

		public AnswerInlineQueryRequest(string inlineQueryId, IEnumerable<InlineQueryResult> queryResults) {
			InlineQueryId = inlineQueryId;
			Results = queryResults.ToList();
		}

		[JsonProperty("inline_query_id")]
		public string InlineQueryId { get; set; }

		[JsonProperty("results")]
		public IList<InlineQueryResult> Results { get; set; }

		[JsonProperty("cache_time")]
		//TODO: Write a converter to write this out as seconds
		public TimeSpan CacheTime { get; set; }

		[JsonProperty("is_personal")]
		public bool IsPersonal { get; set; }

		[JsonProperty("next_offset")]
		public string NextOffset { get; set; }
	}
}
