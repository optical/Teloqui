using System;
using Newtonsoft.Json;
using Teloqui.Data;
using Teloqui.Serialization;
using Xunit;

namespace Teloqui.Tests.Serialization {
	public class EnumConverterTests : SerializationTestBase {

		protected override JsonSerializerSettings Settings => new JsonSerializerSettings {
			Converters = new JsonConverter[] { new EnumConverter() }
		};

		[Fact]
		public void TestSimpleSerialization() {
			Assert.Equal(@"""_first""", SerializeObject(default(SimpleEnum)));
		}

		[Fact]
		public void TestSimpleSerializationOnNonDefault() {
			Assert.Equal(@"""_second""", SerializeObject(SimpleEnum.Second));
		}

		[Fact]
		public void TestFailsSerializationOnEnumValueWithoutAttribute() {
			Assert.Throws<InvalidOperationException>(() => SerializeObject(BrokenEnum.Invalid));
		}

		[Fact]
		public void TestSerializesObjectWithDefaultCorrectly() {
			var originalObject = new SimpleClass();
			var result = SerializeObject(originalObject);
			Assert.Equal(@"{""Value"":""_first""}", result);
		}

		[Fact]
		public void TestSerializesObjectWithExplicitValueCorrectly() {
			var originalObject = new SimpleClass {
				Value = SimpleEnum.Second
			};
			var result = SerializeObject(originalObject);
			Assert.Equal(@"{""Value"":""_second""}", result);
		}

		[Fact]
		public void TestDeserializesDefaultValueCorrectly() {
			Assert.Equal(SimpleEnum.First, DeserializeObject<SimpleEnum>(@"""_first"""));
		}

		[Fact]
		public void TestDeserializesNonDefaultValueCorrectly() {
			Assert.Equal(SimpleEnum.Second, DeserializeObject<SimpleEnum>(@"""_second"""));
		}

		[Fact]
		public void TestDeserializesObjectWithDefaultEnum() {
			var json = @"{""Value"":""_first""}";
			Assert.Equal(SimpleEnum.First, DeserializeObject<SimpleClass>(json).Value);
		}

		[Fact]
		public void TestDeserializesObjectWithNonDefaultEnum() {
			var json = @"{""Value"":""_third""}";
			Assert.Equal(SimpleEnum.Third, DeserializeObject<SimpleClass>(json).Value);
		}

		[Fact]
		public void TestFailsDeserializationOnEnumValueWithoutAttribute() {
			Assert.Throws<InvalidOperationException>(() => DeserializeObject<BrokenEnum>(@"""Invalid"""));
		}

		[Fact]
		public void TestFailsDeserializationOnEnumValueWithAttribute() {
			Assert.Throws<InvalidOperationException>(() => DeserializeObject<BrokenEnum>(@"""_valid_"""));
		}

		[JsonObject]
		private class SimpleClass {
			[JsonProperty]
			public SimpleEnum Value { get; set; }
		}

		private enum SimpleEnum {
			[EnumMember(Value = "_first")]
			First,
			[EnumMember(Value = "_second")]
			Second,
			[EnumMember(Value = "_third")]
			Third
		}

		private enum BrokenEnum {
			[EnumMember(Value = "_valid_")]
			Valid,
			Invalid
		}

	}
}
