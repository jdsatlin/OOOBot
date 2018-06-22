using System;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Json;

namespace PostToSlack
{
	partial class Program
	{
		private static void Main(string[] args)
		{
		//	PostToSlack();
			var listener = Listen();
			Console.WriteLine(listener);
			var messageDict = new MessageSplitter(listener);
			foreach (var pair in messageDict.MessageDictionary)
			{
				Console.WriteLine(pair);
			}


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

		private static string Listen()
		{
			const string boundUrl = "http://*:8181/";
			

			
			using (var listener = new HttpListener())
			{
				listener.Prefixes.Add(boundUrl);
				var prefixes = listener.Prefixes;
				foreach (var prefix in prefixes)
				{
					Console.WriteLine("The prefix(es) are :" + prefix.ToString());
				}

				listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
				listener.Start();
				if (listener.IsListening)
					Console.WriteLine("Listening...");

				HttpListenerContext context =  listener.GetContext();
				
				HttpListenerRequest request = context.Request;
				if (request != null)
					Console.WriteLine("Got a request");
				else
					new ArgumentNullException();

				context.Response.StatusCode = 200;

				var reader = new MessageReader(request.InputStream);
				reader.ReadMessage();
				
						

				context.Response.Close();
				listener.Stop();
				return reader.GetReadMessage();
			}

			
		}
	}
}
