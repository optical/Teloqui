using System;
using Newtonsoft.Json;

namespace Teloqui.Serialization {
	public class TimeSpanToSecondsConverter : JsonConverter {
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			if (value == null) {
				writer.WriteNull();
			} else {
				writer.WriteValue((int)((TimeSpan)value).TotalSeconds);
			}
		}

		public override bool CanConvert(Type objectType) {
			return objectType == typeof (TimeSpan) || objectType == typeof (TimeSpan?);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
			if (reader.Value == null) {
				return null;
			} else {
				return TimeSpan.FromSeconds(Convert.ToInt32(reader.Value));
			}
		}
	}
}
