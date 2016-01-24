using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Teloqui.Data;

namespace Teloqui.Client.SendMessage {
    using MessageResponse = Task<ApiResponse<Message>>;
    using ParameterList = Dictionary<string, string>;

    enum ReplyMarkup {
        None,
        ReplyKeyboardMarkup,
        ReplyKeyboardHide,
        ForceReply
    }

    internal partial class Bot {
        public MessageResponse SendMessageAsync(
            string destinationId,
            string message,
            string messageModifier) {
            return SendMessageAsync(destinationId, message, messageModifier, null, null, false, ReplyMarkup.None, null);
        }

        public MessageResponse SendMessageAsync(
            string destinationId,
            string message,
            bool? disableWebPagePreview) {
            return SendMessageAsync(destinationId, message, string.Empty, disableWebPagePreview, null, false,
                ReplyMarkup.None, null);
        }

        public MessageResponse SendMessageAsync(
            string destinationId,
            string message,
            int? replyToMessageId) {
            return SendMessageAsync(destinationId, message, string.Empty, null, replyToMessageId, false,
                ReplyMarkup.None, null);
        }

        public MessageResponse SendMessageAsync(
            string destinationId,
            string message,
            bool isMarkdown) {
            return SendMessageAsync(destinationId, message, string.Empty, null, null, isMarkdown, ReplyMarkup.None, null);
        }

        public MessageResponse SendMessageAsync(
            string destinationId,
            string message,
            ReplyMarkup replyToMarkup) {
            return SendMessageAsync(destinationId, message, string.Empty, null, null, false, replyToMarkup, null);
        }

        public MessageResponse SendMessageAsync(
            string destinationId,
            string message,
            string messageModifier,
            int? replyToMessageId) {
            return SendMessageAsync(destinationId, message, messageModifier, null, replyToMessageId, false,
                ReplyMarkup.None, null);
        }

        public MessageResponse SendMessageAsync(
            string destinationId,
            string message,
            string messageModifier,
            bool? disableWebPagePreview,
            int? replytoMessageId,
            bool isMarkdown,
            ReplyMarkup replyToMarkup,
            CancellationToken? cancellationToken) {
            var parameters = new ParameterList() {
                ["text"] = message
            };
            if (isMarkdown) {
                parameters["parse_mode"] = "Markdown";
            }
            //Possible System.InvalidOperationException - Please review the use of Cancellation Token here as a non Optional parameter.
            return SendMessageInternal("sendMessage", destinationId, parameters, disableWebPagePreview, replytoMessageId,
                replyToMarkup, cancellationToken.Value);
        }
    }
}