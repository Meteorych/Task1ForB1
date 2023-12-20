using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Task1B1
{
	public static class FilesToSql
	{
		/// <summary>
		/// Method for import of strings to PostgreSQL
		/// </summary>
		/// <param name="path">Path to the file with strings</param>
		/// <param name="logger">Logger to log action of importing</param>
		public static void SqlImport(string path, ILogger logger)
		{
			var linesCount = File.ReadAllLines(path).Length;
			using var con = Program.GetConnection();
			con.Open();
			using var reader = new StreamReader(path);
			var importedCount = 0;
			while (reader.ReadLine() is { } line && CheckString(line))
			{
				var properties = line.Split("||");
				using (var command = new NpgsqlCommand())
				{
					command.Connection = con;
					command.CommandType = CommandType.Text;
					command.CommandText = $"INSERT INTO table_strings (value1, value2, value3, value4, value5) VALUES (@value1, @value2, @value3, @value4, @value5)";
					command.Parameters.AddWithValue("@value1", DateOnly.Parse(properties[0].Trim()));
					command.Parameters.AddWithValue("@value2", properties[1]);
					command.Parameters.AddWithValue("@value3", properties[2]);
					command.Parameters.AddWithValue("@value4", properties[3]);
					command.Parameters.AddWithValue("@value5", properties[4]);
					command.ExecuteNonQuery();
				}
				importedCount++;
				if (importedCount % 1000 == 0)
				{
					logger.Log($"Lines imported: {importedCount} out of {linesCount}");
				}
				
			}
		}

		private static bool CheckString(string fileLine)
		{
			var reg = new Regex(@"^[0-9]{2}/[0-9]{2}/[0-9]{4}||[A-z]{10}||[\p{IsCyrillic}]{10}||[0-9]{1,9}||[0-1]{1}[0-9]{1},[0-9]{8}\n$");
			return reg.IsMatch(fileLine);
		}
	}
}
