using System.Data;

namespace Task1B1
{
	public class SqlManipulation
	{
		/// <summary>
		/// Method to call SQL function that returns sum of integers and median of floats 
		/// </summary>
		/// <param name="logger">Custom logger</param>
		public static void CountingColumns(ILogger logger)
		{
			var con = Program.GetConnection();
			con.Open();
			var command = con.CreateCommand();
			command.CommandType = CommandType.Text;
			command.CommandText = "SELECT * FROM calculate_sum_and_median()";
			using var reader = command.ExecuteReader();
			if (!reader.Read()) return;
			var sumOfInteger = reader.GetInt64(0);
			var medianOfFloats = reader.GetDouble(1);
			logger.Log($"Sum of integers: {sumOfInteger}, median of floats: {medianOfFloats}");

		}
	}
}
