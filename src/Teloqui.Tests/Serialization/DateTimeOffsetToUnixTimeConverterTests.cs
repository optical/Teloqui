using System;
using Newtonsoft.Json;
using Teloqui.Serialization;
using Xunit;

namespace Teloqui.Tests.Serialization {
	public class DateTimeOffsetToUnixTimeConverterTests : SerializationTestBase {
		private static DateTimeOffset UnixEpoch { get; } = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
		
		protected override JsonSerializerSettings Settings => new JsonSerializerSettings {
			Converters = new JsonConverter[] { new DateTimeOffsetToUnixTimeConverter(), }
		};

		[Fact]
		public void TestCanSerializeNull() {
			Assert.Equal("null", SerializeObject<DateTimeOffset?>(null));
		}

		[Fact]
		public void TestCanDeserializeNull() {
			Assert.Equal(null, DeserializeObject<DateTimeOffset?>("null"));
		}

		[Fact]
		public void CanSerializeValue() {
			var value = new DateTimeOffset(2015, 12, 25, 04, 01, 55, TimeSpan.Zero);
			Assert.Equal(CalculateValue(value).ToString(), SerializeObject(value));
		}

		[Fact]
		public void CanDeserializeValue() {
			var value = new DateTimeOffset(2012, 02, 11, 21, 05, 13, TimeSpan.Zero);
			Assert.Equal(value, DeserializeObject<DateTimeOffset>(CalculateValue(value).ToString()));
		}

		[Fact]
		public void TestCanSerializeObjectWithNull() {
			var data = new SimpleClass();
			Assert.Equal(@"{""NormalDateTime"":" + CalculateValue(data.NormalDateTime) + @",""NullableDateTime"":null}", SerializeObject(data));
		}

		[Fact]
		public void TestCanDeserializeObjectWithNull() {
			var data = new SimpleClass();
			var deserializedObject = DeserializeObject<SimpleClass>(@"{""NormalDateTime"":" + CalculateValue(data.NormalDateTime) + @",""NullableDateTime"":null}");

			Assert.Equal(data.NormalDateTime, deserializedObject.NormalDateTime);
			Assert.Equal(data.NullableDateTime, deserializedObject.NullableDateTime);
		}

		[Fact]
		public void TestCanSerializeObjectWithNullableAssigned() {
			var data = new SimpleClass {
				NullableDateTime = new DateTimeOffset(2016, 04, 11, 14, 59, 59, TimeSpan.Zero),
				NormalDateTime = new DateTimeOffset(2015, 4, 12, 5, 03, 59, TimeSpan.Zero)
			};
			var normalValue = CalculateValue(data.NormalDateTime);
			var nullableValue = CalculateValue(data.NullableDateTime.Value);

			Assert.Equal(@"{""NormalDateTime"":" + normalValue + @",""NullableDateTime"":" + nullableValue + "}", SerializeObject(data));
		}

		[Fact]
		public void TestCanDeserializeObjectWithNullableAssigned() {
			var data = new SimpleClass {
				NullableDateTime = new DateTimeOffset(2016, 04, 11, 14, 59, 59, TimeSpan.Zero),
				NormalDateTime = new DateTimeOffset(2015, 4, 12, 5, 03, 59, TimeSpan.Zero)
			};
			var normalValue = CalculateValue(data.NormalDateTime);
			var nullableValue = CalculateValue(data.NullableDateTime.Value);

			var deserializdeObject = DeserializeObject<SimpleClass>(@"{""NormalDateTime"":" + normalValue + @",""NullableDateTime"":" + nullableValue + "}");
			Assert.Equal(data.NormalDateTime, deserializdeObject.NormalDateTime);
			Assert.Equal(data.NullableDateTime, deserializdeObject.NullableDateTime);
		}


		private long CalculateValue(DateTimeOffset offset) {
			return (long)(offset - UnixEpoch).TotalSeconds;
		}

		[JsonObject]
		private class SimpleClass  {
			private static readonly DateTimeOffset DefaultTime = new DateTimeOffset(2015, 05, 04, 14, 33, 04, TimeSpan.Zero);

			public SimpleClass() {
				NormalDateTime = DefaultTime;
			}

			[JsonProperty]
			public DateTimeOffset NormalDateTime { get; set; }

			[JsonProperty]
			public DateTimeOffset? NullableDateTime { get; set; }
		}
	}
}
