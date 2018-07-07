using System;
using System.Runtime.CompilerServices;

namespace PostToSlack
{
	public class User : IEquatable<User>
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

		public bool Equals(User other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return string.Equals(_userId, other._userId) && string.Equals(_username, other._username);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			var other = obj as User;
			return other != null && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((_userId != null ? _userId.GetHashCode() : 0) * 397) ^ (_username != null ? _username.GetHashCode() : 0);
			}
		}
	}
}