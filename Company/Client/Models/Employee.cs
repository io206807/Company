using CsvHelper.Configuration.Attributes;

namespace Client.Models
{
	public class Employee
	{
		[Name("chief_id")]
		public int ChiefId { get; set; }

		[Name("department_id")]
		public int DepartmentId { get; set; }

		[Name("id")]
		public int Id { get; set; }

		[Name("name")]
		public string Name { get; set; }

		[Name("salary")]
		public int Salary { get; set; }
	}
}
