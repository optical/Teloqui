using System;
using System.Threading;
using System.Threading.Tasks;
using Mono.Options;

namespace Teloqui.PollingSample {
	public class Program {

		public static int Main(string[] args) {
			return Run(args).Result;
		}

		private static async Task<int> Run(string[] args) {
			string authToken = null;
			bool showHelp = false;
			int offset = 0;
			OptionSet options = new OptionSet {
				{ "t|token=", "telegram auth token for the bot", value => authToken = value },
				{ "h|help", "display this help text", v => showHelp = v != null },
				{ "o|offset", "the update offset to start at when making requests", (int value) => offset = value }
			};

			try {
				options.Parse(args);
			} catch (OptionException exception) {
				DisplayIncorrectUsage(exception.Message);
				return 1;
			}

			if (string.IsNullOrEmpty(authToken)) {
				DisplayIncorrectUsage("Telegram auth token must be specified. Use the --token command-line argument.");
				return 1;
			}

			if (showHelp) {
				DisplayHelp(options);
				return 0;
			}

			var cancellationTokenSource = new CancellationTokenSource();
			Console.CancelKeyPress += (sender, eventArgs) => {
				Console.WriteLine($"Terminating...");
				cancellationTokenSource.Cancel();
				eventArgs.Cancel = true;
			};

			Console.WriteLine($"Starting echo bot with update offset {offset}");
			EchoBot bot = new EchoBot(authToken, offset);
			try {
				await bot.Run(cancellationTokenSource.Token);
			} catch (OperationCanceledException) {
				Console.WriteLine("Graceful shutdown complete");
			}
			return 0;
		}

		private static void DisplayHelp(OptionSet options) {
			Console.WriteLine("This is a demo bot using Teloqui.");
			Console.WriteLine("The only required argument is the auth token");
			Console.WriteLine("Long polling is used for fetching updates from the service.");
			Console.WriteLine("Options:");
			options.WriteOptionDescriptions(Console.Out);
		}

		private static void DisplayIncorrectUsage(string additionalDetails) {
			Console.WriteLine("Incorrect usage!");
			Console.WriteLine(additionalDetails);
			Console.WriteLine("Try using --help for usage information");
		}
	}
}
