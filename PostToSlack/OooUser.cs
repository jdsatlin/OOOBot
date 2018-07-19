namespace PostToSlack
{
	public class OooUser : User
	{
		public OooUser(string userId) : base(userId)
		{

		}

		public override bool GetOooStatus()
		{
			return true; //user is OOO
		}
	}
}