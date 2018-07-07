using System;
using System.Collections.Generic;

namespace PostToSlack
{
	public class ScheduledMessage
	{
		public readonly int Schedule;
		public List<User> ScheduledUsers;
		private readonly DateTime _startTime;

		public ScheduledMessage(int scheduleInMins)
		{
			Schedule = scheduleInMins;
			_startTime = DateTime.Now;
			ScheduledUsers = new List<User>();
		}

		public bool ScheduleChecker()
		{
			if (_startTime > DateTime.Now || Schedule <= 0 )
				throw new InvalidOperationException();
			if (ScheduledUsers.Count == 0)
				return false;
			var span = DateTime.Now - _startTime;
			return span.Minutes % Schedule == 0;

		}



	}
}