using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNet.WebUtilities;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Teloqui.Data;
using Teloqui.Data.InlineMode;

namespace Teloqui.Client {

	using MessageResponse = Task<Message>;
	using ParameterList = Dictionary<string, string>;
	using ReplyMarkup = Object;

	public class Bot {
		private readonly ConcurrentDictionary<TimeSpan, HttpClient> _httpClientCache;
		private readonly JsonSerializer _deserializer;
		private readonly JsonSerializerSettings _serializerSettings;

		public Bot(string authToken) {
			AuthToken = authToken;
			_httpClientCache = new ConcurrentDictionary<TimeSpan, HttpClient>();
			_serializerSettings = new JsonSerializerSettings {
				DefaultValueHandling = DefaultValueHandling.Ignore,
				Converters = { new StringEnumConverter() }
			};

			_deserializer = new JsonSerializer() {
				DefaultValueHandling = DefaultValueHandling.Ignore,
				Converters = { new StringEnumConverter() }
			};
		}

		public string AuthToken { get; }

		/// <summary>
		/// Returns the period added onto the HTTP Request when making a long polling request.
		/// This is used to prevent a TaskCanceledException being raised due to the request taking at least as long as the timeout specified.
		/// Eg a 30 second timeout, with a LongPollingTimeoutBufferPeriod of 5 will take at most 35 seconds. 30 seconds for telegram to handle the request, 5 seconds leniency due to inherent latency
		/// </summary>
		public TimeSpan LongPollingTimeoutBufferPeriod { get; set; } = TimeSpan.FromSeconds(5);

		/// <summary>
		/// Timeout for requests which do not explicitly take in a timeout, ie non-polling requests.
		/// </summary>
		public static TimeSpan StandardRequestTimeout { get; set; } = TimeSpan.FromSeconds(100);

		// Public methods

		public Task<User> GetMeAsync() {
			return PerformGetAsync<User>("getMe");
		}

		public MessageResponse SendMessageAsync(
			Chat destination,
			string message,
			bool? disableWebPagePreview = null,
			int? replyToMessageId = null,
			ReplyMarkup replyToMarkup = null,
			ParseMode? parseMode = null,
			CancellationToken cancellationToken = default(CancellationToken)) {

			return SendMessageAsync(destination.Id, message, disableWebPagePreview, replyToMessageId, replyToMarkup, parseMode, cancellationToken);
		}

		public MessageResponse SendMessageAsync(
			int destinationId,
			string message,
			bool? disableWebPagePreview = null,
			int? replyToMessageId = null,
			ReplyMarkup replyToMarkup = null,
			ParseMode? parseMode = null,
			CancellationToken cancellationToken = default(CancellationToken)) {
			var parameters = new ParameterList {
				["text"] = message
			};

			if (parseMode != null) {
				parameters["parse_mode"] = parseMode.Value.GetMemberAttribute().Value;
			}

			return SendMessageInternal("sendMessage", destinationId, parameters, disableWebPagePreview, replyToMessageId, replyToMarkup, cancellationToken);
		}

		public MessageResponse ForwardMessageAsync(int chatId, int fromChatId, int messageId, CancellationToken cancellationToken = default(CancellationToken)) {
			var parameters = new ParameterList {
				["from_chat_id"] = fromChatId.ToString(),
				["message_id"] = messageId.ToString()
			};

			return SendMessageInternal("forwardMessage", chatId, parameters, cancellationToken: cancellationToken);
		}

		public MessageResponse SendPhotoAsync(
			int chatId,
			string photoId,
			string caption = null,
			int? replyToMessageId = null,
			ReplyMarkup replyToMarkup = null,
			CancellationToken cancellationToken = default(CancellationToken)) {
			var parameters = new ParameterList {
				["photo"] = photoId
			};

			if (caption != null) {
				parameters["caption"] = caption;
			}

			return SendMessageInternal(
				"sendPhoto",
				chatId,
				parameters,
				replyToMessageId: replyToMessageId,
				replyToMarkup: replyToMarkup,
				cancellationToken: cancellationToken);
		}

		public MessageResponse SendAudioAsync(
			int chatId,
			string audioId,
			int? duration = null,
			int? replyToMessageId = null,
			ReplyMarkup replyToMarkup = null,
			CancellationToken cancellationToken = default(CancellationToken)) {
			var parameters = new ParameterList {
				["audio"] = audioId
			};

			if (duration.HasValue) {
				parameters["duration"] = duration.ToString();
			}

			return SendMessageInternal(
				"sendAudio",
				chatId,
				parameters,
				replyToMessageId: replyToMessageId,
				replyToMarkup: replyToMarkup,
				cancellationToken: cancellationToken);
		}

		public MessageResponse SendDocumentAsync(
			int chatId,
			string documentId,
			int? replyToMessageId = null,
			ReplyMarkup replyToMarkup = null,
			CancellationToken cancellationToken = default(CancellationToken)) {
			var parameters = new ParameterList {
				["document"] = documentId
			};

			return SendMessageInternal(
				"sendDocument",
				chatId,
				parameters,
				replyToMessageId: replyToMessageId,
				replyToMarkup: replyToMarkup,
				cancellationToken: cancellationToken);
		}

		public MessageResponse SendStickerAsync(
			int chatId,
			string stickerId,
			int? replyToMessageId = null,
			ReplyMarkup replyToMarkup = null,
			CancellationToken cancellationToken = default(CancellationToken)) {
			var parameters = new ParameterList {
				["sticker"] = stickerId
			};

			return SendMessageInternal(
				"sendSticker",
				chatId,
				parameters,
				replyToMessageId: replyToMessageId,
				replyToMarkup: replyToMarkup,
				cancellationToken: cancellationToken);
		}

		public MessageResponse SendVideoAsync(
			int chatId,
			string videoId,
			int? duration = null,
			string caption = null,
			int? replyToMessageId = null,
			ReplyMarkup replyToMarkup = null,
			CancellationToken cancellationToken = default(CancellationToken)) {
			var parameters = new ParameterList {
				["video"] = videoId
			};

			if (duration.HasValue) {
				parameters["duration"] = duration.ToString();
			}

			if (caption != null) {
				parameters["caption"] = caption;
			}

			return SendMessageInternal(
				"sendVideo",
				chatId,
				parameters,
				replyToMessageId: replyToMessageId,
				replyToMarkup: replyToMarkup,
				cancellationToken: cancellationToken);
		}

		public MessageResponse SendLocationAsync(
			int chatId,
			double latitude,
			double longitude,
			int? duration = null,
			string caption = null,
			int? replyToMessageId = null,
			ReplyMarkup replyToMarkup = null,
			CancellationToken cancellationToken = default(CancellationToken)) {
			var parameters = new ParameterList {
				["latitude"] = latitude.ToString(CultureInfo.InvariantCulture),
				["longitude"] = longitude.ToString(CultureInfo.InvariantCulture)
			};

			return SendMessageInternal(
				"sendLocation",
				chatId,
				parameters,
				replyToMessageId: replyToMessageId,
				replyToMarkup: replyToMarkup,
				cancellationToken: cancellationToken);
		}

		public Task<bool> SendChatUpdateAsync(int chatId, ChatAction action, CancellationToken cancellationToken = default(CancellationToken)) {
			var parameters = new ParameterList {
				["action"] = action.GetType().GetRuntimeField(action.ToString()).GetCustomAttribute<EnumMemberAttribute>().Value,
				["chat_id"] = chatId.ToString()
			};

			return PerformPostAsFormAsync<bool>("sendChatAction", parameters, cancellationToken: cancellationToken);
		}

		public Task<UserProfilePhotos[]> GetUserProfilePhotosAsync(
			int userId,
			int? offset = null,
			int? limit = null,
			CancellationToken cancellationToken = default(CancellationToken)) {

			var parameters = new ParameterList {
				["user_id"] = userId.ToString()
			};

			if (offset.HasValue) {
				parameters["offset"] = offset.ToString();
			}

			if (limit.HasValue) {
				parameters["limit"] = limit.ToString();
			}

			return PerformPostAsFormAsync<UserProfilePhotos[]>("getUserProfilePhotos", parameters, cancellationToken: cancellationToken);
		}

		// TODO: This needs to be implemented as a POST with a JSON body of type AnswerInlineQueryRequest
		public Task AnswerInlineQuery(
			string inlineQueryId,
			IEnumerable<InlineQueryResult> results,
			TimeSpan? cacheTime = null,
			bool? isPersonal = null,
			string nextOffset = null,
			CancellationToken cancellationToken = default(CancellationToken)) {

			var body = new AnswerInlineQueryRequest(inlineQueryId, results);
			body.CacheTime = cacheTime ?? body.CacheTime;
			body.IsPersonal = isPersonal ?? body.IsPersonal;
			body.NextOffset = nextOffset ?? body.NextOffset;

			return PerformPostAsJsonAsync<bool>("answerInlineQuery", body, cancellationToken: cancellationToken);
		}

		public Task<Update[]> GetUpdates(
			int? offset = null,
			long? limit = null,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default(CancellationToken)) {

			var parameters = new ParameterList();
			if (offset.HasValue) {
				parameters["offset"] = offset.ToString();
			}

			if (limit.HasValue) {
				parameters["limit"] = limit.ToString();
			}

			if (timeout != null) {
				parameters["timeout"] = timeout.Value.TotalSeconds.ToString(CultureInfo.InvariantCulture);
			}

			if (!offset.HasValue) {
				return PerformGetAsync<Update[]>("getUpdates", parameters, cancellationToken: cancellationToken);
			}

			// Passing in an offset means the request is no longer idempotent, and so should be POSTed instead.
			return PerformPostAsFormAsync<Update[]>("getUpdates", parameters, timeout + LongPollingTimeoutBufferPeriod, cancellationToken);
		}

		public Task<object> GetFileAsync(string fileId, CancellationToken cancellationToken = default(CancellationToken)) {
			return PerformGetAsync<object>("getFile", null, cancellationToken: cancellationToken);
		}

		public Task<bool> DisableWebHookAsync(CancellationToken cancellationToken = default(CancellationToken)) {
			return PerformPostAsFormAsync<bool>("setWebhook", new ParameterList(), cancellationToken: cancellationToken);
		}

		public Task<bool> SetWebHookAsync(string url, CancellationToken cancellationToken = default(CancellationToken)) {
			return SetWebHookAsync(new Uri(url), cancellationToken);
		}

		public Task<bool> SetWebHookAsync(Uri url, CancellationToken cancellationToken = default(CancellationToken)) {
			var parameters = new ParameterList {
				["url"] = url.ToString()
			};

			return PerformPostAsFormAsync<bool>("setWebhook", parameters, cancellationToken: cancellationToken);
		}

		// Internals
		private MessageResponse SendMessageInternal(
			string method,
			int chatId,
			ParameterList parameters,
			bool? disableWebPagePreview = null,
			int? replyToMessageId = null,
			object replyToMarkup = null,
			CancellationToken cancellationToken = default(CancellationToken)) {
			parameters["chat_id"] = chatId.ToString();

			if (disableWebPagePreview.HasValue) {
				parameters["disable_web_page_preview"] = disableWebPagePreview.ToString();
			}
			if (replyToMessageId.HasValue) {
				parameters["reply_to_message_id"] = replyToMessageId.ToString();
			}

			return PerformPostAsFormAsync<Message>(method, parameters, cancellationToken: cancellationToken);
		}

		private Task<T> PerformGetAsync<T>(
			string method,
			ParameterList parameters = null,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default(CancellationToken)) {

			string uri = method;
			if (parameters != null) {
				uri = QueryHelpers.AddQueryString(uri, parameters);
			}
			return PerformRequestAsync<T>(() => GetHttpClient(timeout).GetAsync(uri, cancellationToken));
		}

		private Task<T> PerformPostAsFormAsync<T>(
			string method,
			IEnumerable<KeyValuePair<string, string>> parameters,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default(CancellationToken)) {

			return PerformRequestAsync<T>(async () => {
				using (var httpContent = new FormUrlEncodedContent(parameters)) {
					return await GetHttpClient(timeout).PostAsync(method, httpContent, cancellationToken).ConfigureAwait(false);
				}
			});
		}

		private Task<T> PerformPostAsJsonAsync<T>(
			string method,
			object body,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default(CancellationToken)) {

			return PerformRequestAsync<T>(() => GetHttpClient(timeout).PostAsync(
				method,
				new StringContent(
					JsonConvert.SerializeObject(body, _serializerSettings),
					Encoding.UTF8,
					"application/json"),
				cancellationToken));
		}

		private async Task<T> PerformRequestAsync<T>(Func<Task<HttpResponseMessage>> messageFactory) {
			HttpResponseMessage message = await messageFactory().ConfigureAwait(false);

			using (Stream stream = await message.Content.ReadAsStreamAsync().ConfigureAwait(false)) {
				using (StreamReader reader = new StreamReader(stream)) {
					using (JsonReader jsonReader = new JsonTextReader(reader)) {
						var response = _deserializer.Deserialize<ApiResponse<T>>(jsonReader);
						if (response.Ok) {
							message.EnsureSuccessStatusCode();
							return response.Result;
						} else {
							throw new TelegramException(response.Description, response.ErrorCode);
						}
					}
				}
			}
		}

		private HttpClient GetHttpClient(TimeSpan? timeout = null) {
			TimeSpan actualTimeout = timeout ?? StandardRequestTimeout;
			return _httpClientCache.GetOrAdd(actualTimeout, timespan => new HttpClient {
				BaseAddress = new Uri($"https://api.telegram.org/bot{AuthToken}/"),
			});
		}
	}
}
