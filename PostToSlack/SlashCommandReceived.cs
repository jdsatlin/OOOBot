using System.Runtime.Serialization;

namespace PostToSlack
{
	[DataContract]
	class SlashCommandReceived
	{
		[DataMember] internal string Token;
		[DataMember] internal string TeamId;
		[DataMember] internal string TeamDomain;
		[DataMember] internal string EnterpriseId;
		[DataMember] internal string EnterpriseName;
		[DataMember] internal string ChannelId;
		[DataMember] internal string ChannelName;
		[DataMember] internal string UserId;
		[DataMember] internal string UserName;
		[DataMember] internal string Command;
		[DataMember] internal string Text;
		[DataMember] internal string ResponseUrl;
		[DataMember] internal string TriggerId;
	}
}