using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using Timer = System.Threading.Timer;

namespace PostToSlack
{
	public class MessageSchedule
	{
		private readonly List<DateTime> _messageTimes;
		private List<Timer> _timers;

		private MessageSchedule()
		{
			_messageTimes = new List<DateTime>();
			_timers = new List<Timer>();
		}

		public MessageSchedule(DateTime dailyMessageTime) : this()
		{
			_messageTimes.Add(dailyMessageTime);
			var timer = new Timer(Callback);
		}
	

		public MessageSchedule(List<DateTime> dailyMessageTimes) : this()
		{
			_messageTimes.AddRange(dailyMessageTimes);
			foreach (var time in _messageTimes)
			{
				
			}
		}

		private void Callback(object state)
		{
			throw new NotImplementedException();
		}


	}

	public class ScheduledMessage
	{
		private readonly List<User> _scheduledUsers;
		public IReadOnlyCollection<User> ScheduledUsers => _scheduledUsers.AsReadOnly();
		private TimeSpan RepostDays = TimeSpan.FromDays(1);
		private bool _isReadyToPost;
		public bool IsReadyToPost => _isReadyToPost;
		private string _message;
		public string Message
		{
			get
			{
				if (String.IsNullOrWhiteSpace(_message))
					throw new InvalidOperationException("Message was empty");
				return _message;
			}
			set => _message = value;
		}

		private ScheduledMessage()
		{
			_scheduledUsers = new List<User>();
		}

		public ScheduledMessage(DateTime messageTime)
		: this()
		{
			var timer = new Timer(Callback);
			var initialInterval = DetermineNextDailyOccurence(messageTime);
			timer.Change(initialInterval, RepostDays);
		}

		private void Callback(object state)
		{
			if (ScheduledUsers.Count == 0)
			{
				return;
			}
			var builder = new StringBuilder();
			foreach (var user in ScheduledUsers)
			{
				builder.AppendLine($"{user.GetUsername()} is {(user.GetStatus() ? "OOO" : "Not OOO")}");
			}

			var slackMessage = new { text = builder.ToString() };
			Console.WriteLine("slackMessage is " + slackMessage);
			var ser = new JavaScriptSerializer();
			var message = ser.Serialize(slackMessage);
			Console.WriteLine(message);
			Message = message;
			// This really should call the post itself right here. However, that currently requires providing the post to slack an HTTP client
			// So until PostToSlack is an httpclient, this will use bool _isReadyToPost instead.
			_isReadyToPost = true;
		}

		//private TimeSpan DetermineInitialInterval(DateTime messageTime)
		//{
		//	DateTime timeZoneCorrected = messageTime.ToUniversalTime();
		//	var currentTime = DateTime.UtcNow;
		//	DateTime yearCorrected;
		//	if (timeZoneCorrected.Year < currentTime.Year)
		//		yearCorrected = timeZoneCorrected.AddYears(currentTime.Year - timeZoneCorrected.Year);
		//	else if (messageTime.Year > currentTime.Year)
		//		yearCorrected = messageTime.AddYears(timeZoneCorrected.Year - currentTime.Year);
		//	else
		//		yearCorrected = timeZoneCorrected;

		//	DateTime dateCorrected;
		//	if (yearCorrected.DayOfYear < currentTime.DayOfYear)
		//		dateCorrected = yearCorrected.AddDays(currentTime.DayOfYear - yearCorrected.DayOfYear);
		//	else if (yearCorrected.DayOfYear > currentTime.DayOfYear)
		//		dateCorrected = yearCorrected.AddDays(yearCorrected.DayOfYear - currentTime.DayOfYear;
		//	else
		//		dateCorrected = yearCorrected;

		//	DateTime timeOfDayCorrected = (dateCorrected.TimeOfDay > currentTime.TimeOfDay)
		//		? dateCorrected.AddDays(1)
		//		: dateCorrected;

		//	return timeOfDayCorrected - currentTime;

		//}

		private TimeSpan DetermineNextDailyOccurence(DateTime messageTime)
		{
			var span = messageTime.ToUniversalTime().Subtract(DateTime.UtcNow);
			span = span.Subtract(new TimeSpan(span.Days, 0, 0, 0, 0));

			if (span.Ticks > 0)
				return span;
			else
				return span.Add(new TimeSpan(1, 0, 0, 0));
		}

		public void AddScheduledUser(User user)
		{
			if (!user.GetStatus())
				throw new InvalidOperationException("Cannot add non-OOO user to the scheduled message");
			if (!_scheduledUsers.Contains(user))
			_scheduledUsers.Add(user);
			else
			{
				var userMatch = _scheduledUsers.Find(u => u.GetUsername() == user.GetUsername());
					_scheduledUsers.Remove(userMatch);
					_scheduledUsers.Add(user);
			}

		}
		public void PostCompleted()
		{
			_isReadyToPost = false;
		}
	}
}