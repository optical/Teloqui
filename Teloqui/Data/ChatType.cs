namespace Teloqui.Data {

	public enum ChatType {

		[EnumMember(Value = "private")]
		Private,

		[EnumMember(Value = "group")]
		Group,

		[EnumMember(Value = "supergroup")]
		SuperGroup,

		[EnumMember(Value = "channel")]
		Channel,
	}

}
