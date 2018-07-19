using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
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
using System.Web.UI.WebControls;
using Timer = System.Timers.Timer;

namespace PostToSlack
{
	internal partial class Program
	{

		private static void Main(string[] args)
		{
			Options.ReadOptionsFromFile();
			Options.ApplyOptions();
			var listenerBehavior = new ListenerBehavior();
			listenerBehavior.CurrentMessage = new ScheduledMessage(Options.PostingTime);


			using (var client = new HttpClient())
			{
				using (var listener = new HttpListener())
				{
					ListenerBehavior.SetHttpListenerOptions(listener, Options.GetBinding());
					listener.Start();
					var timer = new Timer(60 * 1000);
					timer.Start();

					timer.Elapsed += delegate (object o, ElapsedEventArgs e)
					{
						FlatFileStorage.SaveUsers(listenerBehavior.CurrentMessage.ScheduledUsers.ToList());
						if (!listenerBehavior.CurrentMessage.IsReadyToPost) return;
						var poster = new PostToSlack(client);
						var post = poster.Post(listenerBehavior.CurrentMessage.Message);
						Console.WriteLine(post.Result);
						listenerBehavior.CurrentMessage.PostCompleted();

					};

					IAsyncResult rawResult = listener.BeginGetContext(listenerBehavior.ListenerCallback, listener);
					Console.ReadLine();
				}

			}

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

				var reader = new Program.MessageReader(request.InputStream);
				reader.ReadMessage();
				

				context.Response.Close();
				return reader.Body;

		}
	}
}
