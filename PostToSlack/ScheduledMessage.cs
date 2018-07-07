﻿using System;
using System.Collections.Generic;

namespace PostToSlack
{
	public class ScheduledMessage
	{
		public readonly int Schedule;
		private readonly DateTime _startTime;
		private List<User> _scheduledUsers;
		public IReadOnlyCollection<User> ScheduledUsers => _scheduledUsers.AsReadOnly();

		public ScheduledMessage(int scheduleInMins)
		{
			Schedule = scheduleInMins;
			_startTime = DateTime.Now;
			_scheduledUsers = new List<User>();
		}

		public void AddScheduledUser(User user)
		{
			if (!_scheduledUsers.Contains(user))
			_scheduledUsers.Add(user);
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