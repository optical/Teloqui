using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Teloqui.Data;

namespace Teloqui.Client.SendMessage
{
    internal partial class Bot
    {
        private readonly JsonSerializer _serializer;

        private async Task<ApiResponse<T>> PerformRequest<T>(Func<Task<HttpResponseMessage>> messageFactory) {
            HttpResponseMessage message = await messageFactory();
            message.EnsureSuccessStatusCode();

            using (Stream stream = await message.Content.ReadAsStreamAsync()) {
                using (StreamReader reader = new StreamReader(stream)) {
                    using (JsonReader jsonReader = new JsonTextReader(reader)) {
                        return _serializer.Deserialize<ApiResponse<T>>(jsonReader);
                    }
                }
            }
        }
    }
}
