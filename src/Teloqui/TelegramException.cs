using System;

namespace Teloqui {
	/// <summary>
	/// Raised whenever a telegram method returns a response with "ok" as false. Should generally not occur
	/// Details about the error can be found on the standard Message property of the exception.
	/// </summary>
	public class TelegramException : Exception {

		/// <summary>
		/// The error code on the response object returned by telegram. Is not from the HTTP headers as telegram have not documented it.
		/// </summary>
		public int? ErrorCode { get; }

		public TelegramException(string description, int? errorCode) : base(description) {
			ErrorCode = errorCode;
		}
	}
}
