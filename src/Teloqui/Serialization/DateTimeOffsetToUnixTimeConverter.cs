using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Teloqui.Serialization {
	public class DateTimeOffsetToUnixTimeConverter : JsonConverter {
		private static DateTimeOffset UnixEpoch { get; } = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			if (value == null) {
				writer.WriteNull();
			} else {
				writer.WriteValue((long)((DateTimeOffset)value - UnixEpoch).TotalSeconds);
			}
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
			if (reader.Value == null) {
				return null;
			} else {
				return UnixEpoch.AddSeconds(Convert.ToUInt64(reader.Value));
			}
		}

		public override bool CanConvert(Type objectType) {
			return typeof (DateTimeOffset) == objectType || typeof(DateTimeOffset?) == objectType;
		}
	}
}
