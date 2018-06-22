using System;
using System.Collections.Generic;
using System.Linq;

namespace PostToSlack
{
	public class MessageSplitter
	
	{
		public Dictionary<string, string> MessageDictionary;

		public MessageSplitter(string message)
		{
			if (String.IsNullOrWhiteSpace(message))
				new InvalidOperationException("String passed to MessageSplitter was empty.");

			MessageDictionary = StringToDictionary(message);
		}

		
		public static Dictionary<string, string> StringToDictionary(string line)
		{
			char stringSplit = '&';
			char keyValueSplit = '=';
			return line.Split(new[] { stringSplit }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Split(new[] { keyValueSplit })).ToDictionary(x => x[0], y => y[1]);
		}
	}
}