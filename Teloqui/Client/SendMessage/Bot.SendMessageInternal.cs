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

namespace Teloqui.Client.SendMessage
{
    using MessageResponse = Task<ApiResponse<Message>>;
    using ParameterList = Dictionary<string, string>;
    using ReplyMarkup = Object;
    internal partial class Bot {

        private MessageResponse SendMessageInternal(
            string method,
            string chatId,
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
    }
}
