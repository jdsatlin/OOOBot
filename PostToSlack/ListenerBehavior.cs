using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace PostToSlack
{
	internal class ListenerBehavior
	{
		public ScheduledMessage CurrentMessage;

		public void ListenerCallback(IAsyncResult result)
		{
			HttpListener listener = (HttpListener) result.AsyncState;
			HttpListenerContext context = listener.EndGetContext(result);
			HttpListenerRequest request = context.Request;

			HttpListenerResponse response = context.Response;
			response.StatusCode = 200;
		

			var reader = new Program.MessageReader(request.InputStream);
			reader.ReadMessage(); 
			BuildandAddUsers(reader.Body, CurrentMessage);
			response.Close();

			var nextIteration = listener.BeginGetContext(ListenerCallback, listener);


		}

		private static void BuildandAddUsers(string message, ScheduledMessage scheduledMessage)
		{
			var messageDict = new PostBodyDictBuilder(message);
			foreach (var pair in messageDict.MessageDictionary) Console.WriteLine(pair);

			var user = new OooUser(messageDict.MessageDictionary["user_id"]);
			user.SetUsername(messageDict.MessageDictionary["user_name"]);
			if (messageDict.MessageDictionary["text"].Contains("return"))
			{
				scheduledMessage.RemoveScheduledUser(user);
			}
			scheduledMessage.AddScheduledUser(user);
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