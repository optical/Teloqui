using System;
using Newtonsoft.Json;
using Teloqui.Serialization;
using Xunit;

namespace Teloqui.Tests.Serialization {
	public class TimeSpanToSecondsConverterTests : SerializationTestBase {
		protected override JsonSerializerSettings Settings => new JsonSerializerSettings {
			Converters = new JsonConverter[] { new TimeSpanToSecondsConverter() }
		};

		[Fact]
		public void TestSimpleSerialization() {
			Assert.Equal("120", SerializeObject(TimeSpan.FromSeconds(120)));
		}

		[Fact]
		public void TestFractionalSerialization() {
			Assert.Equal("110", SerializeObject(new TimeSpan(0, 0, 0, 110, 500)));
		}

		[Fact]
		public void TestNullableSerialization() {
			Assert.Equal(@"{""Value"":null}", SerializeObject(new SimpleClassWithNullable()));
		}

		[Fact]
		public void TestNullableWithValueSerialization() {
			Assert.Equal(@"{""Value"":30}", SerializeObject(new SimpleClassWithNullable {
				Value = TimeSpan.FromSeconds(30)
			}));
		}
		[Fact]
		public void TestSimpleClassSerialization() {
			Assert.Equal(@"{""Value"":45}", SerializeObject(new SimpleClass() {
				Value = TimeSpan.FromSeconds(45)
			}));
		}

		[Fact]
		public void TestSimpleDeserialize() {
			Assert.Equal(TimeSpan.FromSeconds(120), DeserializeObject<TimeSpan>("120"));
		}

		[Fact]
		public void TestNullDeserialize() {
			Assert.Equal(null, DeserializeObject<TimeSpan?>("null"));
		}

		[Fact]
		public void TestSimpleClassDeserialize() {
			Assert.Equal(180, DeserializeObject<SimpleClass>(@"{""Value"":180}").Value.TotalSeconds);
		}

		[Fact]
		public void TestSimpleClassWithNullableDeserialize() {
			Assert.Equal(90, DeserializeObject<SimpleClassWithNullable>(@"{""Value"":90}").Value.Value.TotalSeconds);
		}

		[Fact]
		public void TestSimpleClassWithNullableAsNullDeserialize() {
			Assert.Equal(null, DeserializeObject<SimpleClassWithNullable>(@"{""Value"":null}").Value);
		}

		[JsonObject]
		private class SimpleClassWithNullable {
			[JsonProperty]
			public TimeSpan? Value { get; set; }
		}

		[JsonObject]
		private class SimpleClass {
			[JsonProperty]
			public TimeSpan Value { get; set; }
		}
	}
}
