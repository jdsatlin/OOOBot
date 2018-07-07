using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Web.Script.Serialization;
using System.Web.UI;
using Timer = System.Timers.Timer;

namespace PostToSlack
{
	internal partial class Program
	{
		private static readonly ScheduledMessage ScheduledMessage = new ScheduledMessage(1);

		private static void Main(string[] args)
		{
			const string boundUrl = "http://*:8181/";
			
		

			using (var client = new HttpClient())
			{
				using (var listener = new HttpListener())
				{
					SetHttpListenerOptions(listener, boundUrl);
					listener.Start();
					var timer = new Timer(60 * 1000);
					timer.Start();
					ElapsedEventHandler handler = null;
					//var userList = new List<User>();
					//var scheduledMessage = new ScheduledMessage(5);
					while (true)
					{
						IAsyncResult rawResult = listener.BeginGetContext(ListenerCallback, listener);
					
						timer.Elapsed += handler = delegate(object o, ElapsedEventArgs e)
						{
							if (ScheduledMessage.ScheduleChecker())
							{
								var postToSlack = PostToSlack(client, ScheduledMessage.ScheduledUsers);
								Console.WriteLine(postToSlack.Result);
							}

						};
					}
				}

			}

		}

		//private static void CheckTimer()
		//{
		//	if (ScheduledMessage.ScheduleChecker() && ScheduledMessage.ScheduledUsers.Count > 0)
		//	{
		//		var postToSlack = PostToSlack(client, ScheduledMessage.ScheduledUsers);
		//		Console.WriteLine(postToSlack.Result);
		//	}
		//}

	

		private static async Task<string> PostToSlack(HttpClient client, User user)
		{
			var url = new Uri("https://hooks.slack.com/services/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");
			var slackMessage = new { text = $"{user.GetUsername()} is {(user.GetStatus() ? "OOO" : "Not OOO")}"};
			var ser = new JavaScriptSerializer();
			var message = ser.Serialize(slackMessage);
			Console.WriteLine(message);
			var content = new StringContent(message, Encoding.UTF8, "application/json");
			var result = await client.PostAsync(url, content);
			var response = result.StatusCode.ToString();
			return response;
		}

		private static async Task<string> PostToSlack(HttpClient client, List<User> userList)
		{
			var url = new Uri("https://hooks.slack.com/services/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");
			var builder = new StringBuilder();
			foreach (var user in userList)
			{
				builder.AppendLine($"{user.GetUsername()} is {(user.GetStatus() ? "OOO" : "Not OOO")}");
			}

			var slackMessage = new { text = builder.ToString()};
			Console.WriteLine("slackMessage is " + slackMessage);
			var ser = new JavaScriptSerializer();
			var message = ser.Serialize(slackMessage);
			Console.WriteLine(message);
			var content = new StringContent(message, Encoding.UTF8, "application/json");
			var result = await client.PostAsync(url, content);
			var response = result.StatusCode.ToString();
			return response; 
		}

		

		private static string Listen(HttpListener listener)
		{
		
			if (listener.IsListening)
					Console.WriteLine("Listening...");

				var context = listener.GetContext();

				var request = context.Request;
				if (request != null)
					Console.WriteLine("Got a request");
				else
					throw new NetworkInformationException();

				context.Response.StatusCode = 200;

				var reader = new MessageReader(request.InputStream);
				reader.ReadMessage();
				

				context.Response.Close();
				return reader.Body;

		}

		public static void ListenerCallback(IAsyncResult result)
		{
			HttpListener listener = (HttpListener) result.AsyncState;
			HttpListenerContext context = listener.EndGetContext(result);
			HttpListenerRequest request = context.Request;

			HttpListenerResponse response = context.Response;
			response.StatusCode = 200;
		

			var reader = new MessageReader(request.InputStream);
			reader.ReadMessage(); 
			BuildandAddUsers(reader.Body, ScheduledMessage);
			response.Close();


		}

		private static void BuildandAddUsers(string message, ScheduledMessage scheduledMessage)
		{
			var messageDict = new PostBodyDictBuilder(message);
			foreach (var pair in messageDict.MessageDictionary) Console.WriteLine(pair);

			var user = new OooUser(messageDict.MessageDictionary["user_id"]);
			user.SetUsername(messageDict.MessageDictionary["user_name"]);

			scheduledMessage.ScheduledUsers.Add(user);
		}



		public static void SetHttpListenerOptions(HttpListener listener, string bindingUrl)
		{
			listener.Prefixes.Add(bindingUrl);
			listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
		}

		public static void SetHttpListenerOptions(HttpListener listener, string[] bindingUrl)
		{
			foreach (var url in bindingUrl) listener.Prefixes.Add(url);
			listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
		}
	}
}
