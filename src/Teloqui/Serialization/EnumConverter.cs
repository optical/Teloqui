using System;
using System.Linq;
using Newtonsoft.Json;
using Teloqui.Data;

namespace Teloqui.Serialization {
	public class EnumConverter : JsonConverter {

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			if (value == null) {
				writer.WriteNull();
				return;
			}

			var attribute = ((Enum) value).GetMemberAttribute();

			writer.WriteValue(attribute != null ? attribute.Value : value.ToString());
		}

		public override bool CanConvert(Type objectType) {
			return objectType.IsEnum;
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
			if (reader.TokenType != JsonToken.String) {
				throw new Exception("Non-String value is not allowed");
			}

			bool isNullable = objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof (Nullable<>);
			Type type = isNullable ? Nullable.GetUnderlyingType(objectType) : objectType;

			if (reader.TokenType == JsonToken.Null) {
				if (!isNullable) {
					throw new Exception($"Cannot convert null value to {objectType}.");
				}

				return null;
			}

			var lookup = type.GetFields().Where(field => field.FieldType == type).ToDictionary(
				field => field.GetCustomAttributes(typeof (EnumMemberAttribute), true)
					.Cast<EnumMemberAttribute>()
					.Single().Value,
				field => field.Name);

			var convertedValue = lookup[reader.Value.ToString()];
			if (convertedValue == null) {
				throw new Exception($"Value {reader.Value} of type {type.FullName} Lacks an {nameof(EnumMemberAttribute)} - cannot deserialize!");
			}

			return Enum.Parse(objectType, convertedValue);
		}
	}
}
