using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace PostToSlack
{
	public class PostBodyDictBuilder // I feel like this should implement Dictionary<string, string> but there's a knowledge gap here in doing that

	{
		public Dictionary<string, string> MessageDictionary;

		public PostBodyDictBuilder(string message)
		{
			if (String.IsNullOrWhiteSpace(message))
				throw new InvalidOperationException("String passed to PostBodyDictBuilder was empty.");
			

			MessageDictionary = StringToDictionary(message);
			DictUrlDecoder(MessageDictionary);
		}

		
		public Dictionary<string, string> StringToDictionary(string line)
		{
			char stringSplit = '&';
			char keyValueSplit = '=';
			return line.Split(new[] { stringSplit }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Split(new[] { keyValueSplit })).ToDictionary(x => x[0], y => y[1]);
		}

		public string ReplaceLineBreaksWithBlank(string input)
		{
			return Regex.Replace(input, @"\r\n?|\n", "");
		}

		public void DictUrlDecoder(Dictionary<string, string> urlEncodedDictionary)
		{
			var dictCopy = new Dictionary<string, string>();
			foreach (var entry in urlEncodedDictionary)
			{
				dictCopy.Add(entry.Key, entry.Value);
			}

			var i = 0;
			while (i <= dictCopy.Count)
			{
				foreach (var valuePair in dictCopy)
				{
					urlEncodedDictionary.Remove(valuePair.Key);
					urlEncodedDictionary.Add(WebUtility.UrlDecode(ReplaceLineBreaksWithBlank(valuePair.Key)), WebUtility.UrlDecode(ReplaceLineBreaksWithBlank(valuePair.Value)));
					i++;
				}
			}
		}
	}
}