using CsvHelper.Configuration.Attributes;

namespace Client.Models
{
	public class Department
	{
		[Name("id")]
		public int Id { get; set; }

		[Name("name")]
		public string Name { get; set; }
	}
}
