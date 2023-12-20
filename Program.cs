using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Task1B1
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var logger = new CustomLogger();
			//var sw = new Stopwatch();
			//sw.Start();
			Parallel.For(0, 100, index =>
			{
				FileOperations.FileCreation($"{index}.txt");
			});
			FileOperations.MergingFilesIntoOne(".", "abc");

			FilesToSql.SqlImport("0.txt", logger);
			SqlManipulation.CountingColumns(logger);
			//sw.Stop();
			//Console.WriteLine(sw.ElapsedMilliseconds);
		}

		/// <summary>
		/// Custom logger for this certain program
		/// </summary>
		public class CustomLogger : ILogger
		{
			public void Log(string message)
			{
				Console.WriteLine(message);
			}
		}

		/// <summary>
		/// Method to establish connection
		/// </summary>
		/// <returns>NpgsqlConnection</returns>
		public static NpgsqlConnection GetConnection()
		{
			return new NpgsqlConnection("server=localhost;username=postgres;port=5433;database=files_strings;password=");
		}
	}
}
