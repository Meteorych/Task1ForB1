using System.Diagnostics;
using Npgsql;

namespace Task1B1
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var sw = new Stopwatch();
			sw.Start();
			Parallel.For(0, 100, index =>
			{
				FileOperations.FileCreation($"{index}.txt");
			});
			FileOperations.MergingFilesIntoOne(".", "abc");

			sw.Stop();
			Console.WriteLine(sw.ElapsedMilliseconds);
		}

		static NpgsqlConnection GetConnection()
		{
			return new NpgsqlConnection("server=localhost;port=5433;user=postgres;password=");
		}
	}
}
