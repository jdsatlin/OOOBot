using System;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography.X509Certificates;

namespace PostToSlack
{
	partial class Program
	{
		private static void Main(string[] args)
		{
			//	PostToSlack();
			var listener = Listen();
			Console.WriteLine(listener);
			var messageDict = new PostBodyDictBuilder(listener);
			foreach (var pair in messageDict.MessageDictionary)
			{
				Console.WriteLine(pair);
			}
			var user = new OooUser(messageDict.MessageDictionary["user_id"]);

		    user.SetUsername(messageDict.MessageDictionary["user_name"]);
			Console.WriteLine();
			Console.WriteLine("{0}'s OOO status is : {1}", user.GetUsername(), user.GetStatus());


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

				SetHttpListenerOptions(listener, boundUrl);


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

		public static void SetHttpListenerOptions(HttpListener listener, string bindingUrl)
		{
			listener.Prefixes.Add(bindingUrl);
			listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
		}
		public static void SetHttpListenerOptions(HttpListener listener, string[] bindingUrl)
		{
			foreach (var url in bindingUrl)
			{
				listener.Prefixes.Add(url);
			}
			listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
		}
	}
}
