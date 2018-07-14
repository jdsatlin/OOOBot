using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace PostToSlack
{
	internal class PostToSlack
	{
		private HttpClient _client;

		private PostToSlack()
		{
			
		}

		public PostToSlack(HttpClient client)
		{
			_client = client;
		}

		public async Task<string> Post(string message)
		{
			var url = new Uri(Options.WebHookUrl);
			var content = new StringContent(message, Encoding.UTF8, "application/json");
			var result = await _client.PostAsync(url, content);
			var response = result.StatusCode.ToString();
			return response; 
		}

		public void ScheduledMessagerCallback(object timerInfo)
		{
			throw new NotImplementedException();
		}
	}
}