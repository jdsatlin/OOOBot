using System;
using System.CodeDom;
using System.IO;
using System.Text;

namespace PostToSlack
{
	partial class Program
	{
		public class MessageReader
		{
			private readonly Stream _message;
			private string _body;
			public string Body
			{
				get => _body;
				private set
				{
					if (String.IsNullOrWhiteSpace(value))
						throw new ArgumentNullException();
					_body = value;
				}
			}

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

			private void ValidateBody(string body)
			{
				if (string.IsNullOrWhiteSpace(Body))
					throw new ArgumentNullException();
			}
		}
	}
}
