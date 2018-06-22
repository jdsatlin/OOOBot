using System.Runtime.Serialization;

namespace PostToSlack
{
	[DataContract]
	class SlackMessage
	{
		[DataMember]
		internal string Text;
	}
}