namespace PostToSlack
{
	public class OooUser : User
	{
		public OooUser(string userId) : base(userId)
		{

		}

		public override bool GetStatus()
		{
			return true; //user is OOO
		}
	}
}