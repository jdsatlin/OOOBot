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
	}
}