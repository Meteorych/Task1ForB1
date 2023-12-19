using System.Text;

namespace Task1B1
{
	public static class FileOperations
	{
		public static void FileCreation(string path)
		{
			const int size = 100_000;
			var lockObject = new object();

			var rand = new Random();
			var builderStrings = new StringBuilder();
			var oldDate = new DateTime(DateTime.Now.Year - 5, DateTime.Now.Month, DateTime.Now.Day);
			
			Parallel.For(0, size, _ =>
			{
				var randomDate = oldDate.AddDays(rand.Next((DateTime.Now - oldDate).Days));
				lock (lockObject)
				{
					builderStrings.Append($"{DateOnly.FromDateTime(randomDate)}||{GenerateLettersRandomString(true)}||{GenerateLettersRandomString(false)}||" +
					                      $"{rand.Next(1, 100_000_000)}||{Math.Round(rand.NextDouble() * (20 - 1) + 1, 8)}||\n");
				}
			});


			string resultStrings;
			lock (lockObject)
			{
				resultStrings = builderStrings.ToString();
			}
			File.WriteAllText(path, resultStrings);

		}

		//Task Management and async -- too slow in this case...

		//public static async Task MergingFilesIntoOneAsync(string directoryPath, string deleteSymbols)
		//{
		//	File.Delete("FullFile.txt");
		//	var inputFiles = Directory.GetFiles(directoryPath, "*.txt");
		//	var listOfStrings = new ConcurrentBag<string>();
		//	
		//	await Task.WhenAll(inputFiles.Select(async file =>
		//	{
		//		var strings = await File.ReadAllLinesAsync(file);
		//		var filtered = strings.AsParallel().AsOrdered()
		//			.Where(oneString => !oneString.Contains(deleteSymbols));

		//		foreach (var oneString in filtered)
		//		{
		//			listOfStrings.Add(oneString);
		//		}
		//	}));

		//	await File.WriteAllLinesAsync("FullFile.txt", listOfStrings);
		//}

		public static void MergingFilesIntoOne(string directoryPath, string deleteSymbols)
		{
			File.Delete("FullFile.txt");
			var inputFiles = Directory.GetFiles(directoryPath, "*.txt");
			var lockObject = new object();
			//Concurrent bag -- too slow perfomance, so im using lock
			var listOfStrings = new List<string>();


			Parallel.ForEach(inputFiles, file =>
			{
				lock (lockObject)
				{
					listOfStrings.AddRange(File.ReadAllLines(file));
				}
			});
			var resultStrings = listOfStrings
				.Where(oneString => !oneString.Contains(deleteSymbols));
			File.WriteAllLines("FullFile.txt", resultStrings);
		}


		private static string GenerateLettersRandomString(bool latin)
		{
			const string latinChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
			const string russianChars = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя";

			var rand = new Random();
			const int length = 10;
			var chars = latin ? latinChars : russianChars;

			return new string(Enumerable.Repeat(chars, length)
				.Select(s => s[rand.Next(s.Length)]).ToArray());


		}
	}
}
