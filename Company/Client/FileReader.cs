using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;

namespace Client
{
	static class FileReader<T>
	{
		public static async Task<List<T>> ReadAsync(string fileName)
		{
			var list = new List<T>();
			using var reader = new StreamReader(fileName);
			using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
			await foreach(var record in csv.GetRecordsAsync<T>())
			{
				list.Add(record);
			}

			return list;
		}
	}
}
