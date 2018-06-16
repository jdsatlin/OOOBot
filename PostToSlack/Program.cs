using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Net.Http;
using System.Runtime.Serialization.Json;

namespace PostToSlack
{
	class Program
	{
		static void Main(string[] args)
		{
			PostToSlack();

			Console.ReadKey();
		}

		static async void PostToSlack()
		{
			var message = new SlackMessage{Text = "hello world"};
			WebRequest req = WebRequest.Create("https://hooks.slack.com/services/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");
			req.Method = "POST";
			req.ContentType = "application/json";

			Stream stream = req.GetRequestStream();
			DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(SlackMessage));
			ser.WriteObject(stream, message);

			stream.Close();

			WebResponse response = req.GetResponse();
			HttpWebResponse httpResponse = (HttpWebResponse) response;
			Console.WriteLine(httpResponse.StatusCode);
		}

		[DataContract]
		class SlackMessage
		{
			[DataMember]
			internal string Text;
		}
	}
}
