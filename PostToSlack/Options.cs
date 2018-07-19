using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace PostToSlack
{
	public static class Options
	{
		private static string Binding;
		private static List<string> WebHookUrls;
		public static string WebHookUrl;
		public static DateTime PostingTime;
		private static readonly Dictionary<string, string> OptionsFromFile;

		static Options()
		{
			OptionsFromFile = new Dictionary<string, string>();
		}

		public static string GetBinding()
		{
			return Binding;
		}

		public static void AddWebHookUrl(string url)
		{
			if (string.IsNullOrWhiteSpace(url))
				throw new InvalidOperationException("Webhook URL cannot be blank");
			if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
				throw new InvalidOperationException("URL String for webhook appears to be malformed.");
			WebHookUrls.Add(url);
		}

		public static void ReadOptionsFromFile()
		{
			
			var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			if (baseDirectory.Contains("bin"))
				{
				 baseDirectory = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\"));
				}
			using (var reader = new StreamReader(@baseDirectory + @"options.txt"))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					var splitLine = line.Split(new [] {':'}, 2);
					OptionsFromFile.Add(splitLine[0], splitLine[1]);
				}
			
			}

		}

		public static void ApplyOptions()
		{
			if (!OptionsFromFile.ContainsKey("Binding") || !OptionsFromFile.ContainsKey("WebHookUrl") || !OptionsFromFile.ContainsKey("PostingTime"))
			{
				throw new InvalidOperationException();
			}

			Binding = OptionsFromFile["Binding"];
			WebHookUrl = OptionsFromFile["WebHookUrl"];
			PostingTime = DateTime.Parse(OptionsFromFile["PostingTime"]);

		}


	}
}