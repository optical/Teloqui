namespace Teloqui.Data {

	public enum ChatAction {

		[EnumMember(Value = "typing")]
		Typing,

		[EnumMember(Value = "upload_photo")]
		UploadingPhoto,

		[EnumMember(Value = "record_video")]
		RecordingVideo,

		[EnumMember(Value = "upload_video")]
		UploadingVideo,

		[EnumMember(Value = "record_audio")]
		RecordingAudio,

		[EnumMember(Value = "upload_audio")]
		UploadingAudio,

		[EnumMember(Value = "upload_document")]
		UploadingDocument,

		[EnumMember(Value = "find_location")]
		FindingLocation
	}
}