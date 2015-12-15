using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Teloqui.Data;

namespace Optigram.Client {

	using MessageResponse = Task<ApiResponse<Message>>;
	using ParameterList = Dictionary<string, string>;
	using ReplyMarkup = Object;

	public class Bot {
		private readonly HttpClient _httpClient;
		private readonly JsonSerializer _serializer;
		
		public Bot(string authToken) {
			AuthToken = authToken;
			_httpClient = new HttpClient() {
				BaseAddress = new Uri($"https://api.telegram.org/bot{authToken}/")
			};
			_serializer = new JsonSerializer {
				Converters = { new StringEnumConverter() }
			};
		}

		public string AuthToken { get; private set; }

		// Public methods

		public Task<ApiResponse<User>> GetMeAsync() {
			return PerformGet<User>("getMe");
		}

		public MessageResponse SendMessageAsync(
			ChatSource destination,
			string message,
			bool? disableWebPagePreview = null,
			int? replyToMessageId = null,
			ReplyMarkup replyToMarkup = null,
			bool isMarkDown = false,
			CancellationToken cancellationToken = default(CancellationToken)) {

			return SendMessageAsync(destination.Id, message, disableWebPagePreview, replyToMessageId, replyToMarkup, isMarkDown, cancellationToken);
		}

		public MessageResponse SendMessageAsync(
			int destinationId,
			string message,
			bool? disableWebPagePreview = null,
			int? replyToMessageId = null,
			ReplyMarkup replyToMarkup = null,
			bool isMarkDown = false,
			CancellationToken cancellationToken = default(CancellationToken)) {
			var parameters = new ParameterList {
				["text"] = message
			};

			if (isMarkDown) {
				parameters["parse_mode"] = "Markdown";
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
				["latitude"] = latitude.ToString(),
				["longitude"] = longitude.ToString()
			};

			return SendMessageInternal(
				"sendLocation",
				chatId,
				parameters,
				replyToMessageId: replyToMessageId,
				replyToMarkup: replyToMarkup,
				cancellationToken: cancellationToken);
		}

		public Task<ApiResponse<bool>> SendChatUpdateAsync(int chatId, ChatAction action, CancellationToken cancellationToken = default(CancellationToken)) {
			var parameters = new ParameterList {
				["action"] = action.GetType().GetRuntimeField(action.ToString()).GetCustomAttribute<EnumMemberAttribute>().Value,
				["chat_id"] = chatId.ToString()
			};

			return PerformPost<bool>("sendChatAction", parameters, cancellationToken);
		}

		public Task<ApiResponse<UserProfilePhotos[]>> GetUserProfilePhotosAsync(
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
			
			return PerformPost<UserProfilePhotos[]>("getUserProfilePhotos", parameters, cancellationToken);
		}

		public Task<ApiResponse<Update>> GetUpdates(CancellationToken cancellationToken = default(CancellationToken)) {
			return PerformGet<Update>("getUpdates", cancellationToken);
		}

		public Task<ApiResponse<object>> GetFileAsync(string fileId, CancellationToken cancellationToken = default(CancellationToken)) {
			return PerformGet<object>("getFile", cancellationToken);
		}

		public Task<ApiResponse<bool>> DisableWebHookAsync(CancellationToken cancellationToken = default(CancellationToken)) {
			return PerformPost<bool>("setWebhook", new ParameterList(), cancellationToken);
		}

		public Task<ApiResponse<bool>> SetWebHookAsync(string url, CancellationToken cancellationToken = default(CancellationToken)) {
			return SetWebHookAsync(new Uri(url), cancellationToken);
		}

		public Task<ApiResponse<bool>> SetWebHookAsync(Uri url, CancellationToken cancellationToken = default(CancellationToken)) {
			var parameters = new ParameterList {
				["url"] = url.ToString()
			};

			return PerformPost<bool>("setWebhook", parameters, cancellationToken);
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

			return PerformPost<Message>(method, parameters, cancellationToken);
		}

		private Task<ApiResponse<T>> PerformGet<T>(string method, CancellationToken cancellationToken = default(CancellationToken)) {
			return PerformRequest<T>(() => _httpClient.GetAsync(method, cancellationToken));
		}

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
