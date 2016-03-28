using System;
using System.Linq;
using System.Reflection;

namespace Teloqui.Data {
	internal static class EnumMemberAttributeExtension {
		internal static EnumMemberAttribute GetMemberAttribute(this Enum enumValue) {
			return enumValue.GetType()
			.GetMember(enumValue.ToString())
			.Single()
			.GetCustomAttributes(typeof(EnumMemberAttribute), true)
			.Cast<EnumMemberAttribute>()
			.SingleOrDefault();
		}
	}
}
