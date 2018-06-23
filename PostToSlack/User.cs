using System;
using System.Runtime.CompilerServices;

namespace PostToSlack
{
	public class User
	{
		private readonly string _userId;
		private string _username;

		public User(string userId)
		{
			_userId = userId;
		}

		public void SetUsername(string username)
		{
			_username = username;
			if (string.IsNullOrWhiteSpace(_username))
			{
				//try and pull down username from slack by userID
				throw new NotImplementedException();
			}
		}

		public string GetUsername()
		{
			return _username;
		}

		public virtual bool GetStatus()
		{
			return false; //not ooo
		}

	}
}