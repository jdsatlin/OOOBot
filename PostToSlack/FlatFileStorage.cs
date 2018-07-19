using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PostToSlack
{
	public static class FlatFileStorage
	{
		private static readonly string FileLoc;

		static FlatFileStorage()
		{
			var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			if (baseDirectory.Contains("bin"))
			{
				baseDirectory = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\"));
			}

			string storagePath = baseDirectory + @"storage.txt";
			if (!File.Exists(storagePath))
			{
				File.Create(storagePath);
			}

			FileLoc = storagePath;
		}

		public static void SaveUsers(List<User> users)
		{
			ClearList();
			if (users == null || !users.Any()) return;

			using (var writer = new StreamWriter(FileLoc))
			{
				foreach (var user in users)
				{
					writer.WriteLine("{0}:{1}", user.GetUsername(), user.GetOooStatus());
				}
			}
			
		}

		public static List<User> LoadUsers()
		{
			var userList = new List<User>();
			using (var reader = new StreamReader(FileLoc))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					var splitline = line.Split(new[] {':'}, 2);
					var ooo = EvaluateString(splitline[1]);
					var userID = splitline[0];
					userList.Add((ooo) ? new OooUser(userID) : new User(userID));
				}

				return userList;
			}
		}

		private static Boolean EvaluateString(string input)
		{
			if (string.IsNullOrWhiteSpace(input)) return false;
			if (input == "true") return true;
			return false;
		}

		private static void ClearList()
		{
			File.WriteAllLines(FileLoc, new []{""});
		}
		

	}
}