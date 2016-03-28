using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Teloqui.Client;
using Teloqui.Data;
using Teloqui.Data.InlineMode;

namespace Teloqui.PollingSample {
	public class EchoBot {
		private static readonly TimeSpan LongPollingPeriod = TimeSpan.FromSeconds(50);

		private readonly Bot _bot;
		private int _currentOffset;

		public EchoBot(string authToken, int startingOffset) {
			_bot = new Bot(authToken);
			_currentOffset = startingOffset;
		}

		public async Task Run(CancellationToken cancellationToken) {
			while (!cancellationToken.IsCancellationRequested) {
				var updates = (await _bot.GetUpdates(_currentOffset, timeout: LongPollingPeriod, cancellationToken: cancellationToken)).ToList();
				Console.WriteLine($"Received {updates.Count} updates.");
				foreach (var update in updates) {
					if (update.Message != null) {
						await HandleMessage(update.Message);
					} else if (update.InlineQuery != null) {
						await HandleInlineQuery(update.InlineQuery);
					} else if (update.ChosenInlineResult != null) {
						HandleChosenInlineResult(update.ChosenInlineResult);
					}
				}
				_currentOffset = updates.Any() ? updates.Max(update => update.UpdateId) + 1 : _currentOffset;
			}
			cancellationToken.ThrowIfCancellationRequested();
		}

		private async Task HandleMessage(Message message) {
			Console.WriteLine($"Message from {message.From.Username}");
			await _bot.SendMessageAsync(message.Destination, $"ECHO: {message.Text}");
		}

		private async Task HandleInlineQuery(InlineQuery query) {
			Console.WriteLine($"Inline query from {query.From.Username}. Query: {query.Query}");
			await _bot.AnswerInlineQuery(query.Id, new InlineQueryResult[] {
				new InlineQueryResultArticle("Test", "Test Message"),
				new InlineQueryResultGif("https://media.giphy.com/media/8zxuWCs0S2iqs/giphy.gif", "https://media.giphy.com/media/8zxuWCs0S2iqs/giphy.gif"), 
			});

		}

		private void HandleChosenInlineResult(ChosenInlineResult chosenInlineResult) {
			Console.WriteLine($"Chosen query result from {chosenInlineResult.From.Username}, Query:\"{chosenInlineResult.Query}\", ResultId: {chosenInlineResult.ResultId}");
		}
	}
}
