using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Teloqui.Data;

namespace Teloqui.Client.SendMessage {

    internal partial class Bot {
        private readonly HttpClient _httpClient;

        private Task<ApiResponse<T>> PerformPost<T>(
            string method,
            IEnumerable<KeyValuePair<string, string>> parameters,
            CancellationToken cancellationToken = default(CancellationToken)) {

            return PerformRequest<T>(() => {
                using (var httpContent = new FormUrlEncodedContent(parameters)) {
                    return _httpClient.PostAsync(method, httpContent, cancellationToken);
                }
            });
        }
    }
}
