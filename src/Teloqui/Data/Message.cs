using System;
using Newtonsoft.Json;
using Teloqui.Serialization;

namespace Teloqui.Data {
	[JsonObject]
	public class Message {

		[JsonProperty("message_id")]
		public int MessageId { get; set; }

		[JsonProperty("from")]
		public User From { get; set; }

		[JsonProperty("date")]
		[JsonConverter(typeof(DateTimeOffsetToUnixTimeConverter))]
		public DateTimeOffset Date { get; set; }

		[JsonProperty("chat")]
		public Chat Destination { get; set; }

		[JsonProperty("forward_from")]
		public User ForwardFrom { get; set; }

		[JsonProperty("forward_date")]
		[JsonConverter(typeof(DateTimeOffsetToUnixTimeConverter))]
		public DateTimeOffset? ForwardDate { get; set; }

		[JsonProperty("reply_to_message")]
		public Message ReplyToMessage { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("audio")]
		public Audio Audio { get; set; }

		[JsonProperty("document")]
		public Document Document { get; set; }

		[JsonProperty("photo")]
		public PhotoSize[] Photos { get; set; }

		[JsonProperty("sticker")]
		public Sticker Sticker { get; set; }

		[JsonProperty("video")]
		public Video Video { get; set; }

		[JsonProperty("contact")]
		public Contact Contact { get; set; }

		[JsonProperty("location")]
		public Location Location { get; set; }

		[JsonProperty("new_chat_participant")]
		public User NewChatParticipant { get; set; }

		[JsonProperty("left_chat_participant")]
		public User LeftChatParticipant { get; set; }

		[JsonProperty("new_chat_title")]
		public string NewChatTitle { get; set; }

		[JsonProperty("new_chat_photo")]
		public PhotoSize[] NewChatPhoto { get; set; }

		[JsonProperty(PropertyName= "delete_chat_photo")]
		public bool? DeleteChatPhoto { get; set; }

		[JsonProperty("group_chat_created")]
		public bool? GroupChatCreated { get; set; }
	}
}