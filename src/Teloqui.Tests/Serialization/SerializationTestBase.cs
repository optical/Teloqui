using Newtonsoft.Json;

namespace Teloqui.Tests.Serialization {
	public abstract class SerializationTestBase {

		protected abstract JsonSerializerSettings Settings { get; }

		protected string SerializeObject<T>(T plainObject) {
			return JsonConvert.SerializeObject(plainObject, Settings);
		}

		protected T DeserializeObject<T>(string value) {
			return JsonConvert.DeserializeObject<T>(value, Settings);
		}
	}
}
