using System;
using System.IO;
using System.Text;

namespace PostToSlack
{
	partial class Program
	{
		public class MessageReader
		{
			private readonly Stream _message;
			public string Body;

			public MessageReader(Stream message)
			{
				_message = message;
			}

			public void ReadMessage()
			{
				using (var reader = new StreamReader(_message))
				{
					var bodyBuilder = new StringBuilder();
					string line;
					while ((line = reader.ReadLine()) != null)
					{
						bodyBuilder.AppendLine(line);
					}

					Body = bodyBuilder.ToString();

				}
			}

			public string GetReadMessage()
			{
				if (Body == null)
				{
					new InvalidOperationException("Message must be read before it can be returned");
					Body = "body was empty";
				}

				return Body;


			}

		}
	}
}
