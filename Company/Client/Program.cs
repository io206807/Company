using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Client.Models;

namespace Client
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var departmentFile = ConfigurationManager.AppSettings["DepartmentFile"];
			var employeeFile = ConfigurationManager.AppSettings["EmployeeFile"];
			List<Department> departments;
			List<Employee> employees;

			try
			{
				departments = await FileReader<Department>.ReadAsync(departmentFile);
				employees = await FileReader<Employee>.ReadAsync(employeeFile);
			}
			catch
			{
				Console.WriteLine("Структура файлов отличается от ожидаемой.");
				return;
			}

			Console.WriteLine("1a. Суммарная зарплата в разрезе департаментов:");
			var totalSalaryByDepartment = employees.GroupBy(e => e.DepartmentId, e => e.Salary).Select(i => new {departmentId = i.Key, sum = i.Sum()})
			                                       .Join(departments, group => group.departmentId, department => department.Id,
			                                             (group, department) => new {id = department.Id, name = department.Name, group.sum});
			foreach(var item in totalSalaryByDepartment)
			{
				Console.WriteLine($"{item.name}: ${item.sum}");
			}

			Separate();

			Console.WriteLine("1b. Суммарная зарплата в разрезе департаментов (без руководителей):");
			var totalSalaryByDepartmentWithoutChief = employees.Where(e => e.ChiefId != 0).GroupBy(e => e.DepartmentId, e => e.Salary)
			                                                   .Select(i => new {departmentId = i.Key, sum = i.Sum()})
			                                                   .Join(departments, group => group.departmentId, department => department.Id,
			                                                         (group, department) => new {id = department.Id, name = department.Name, group.sum});
			foreach(var item in totalSalaryByDepartmentWithoutChief)
			{
				Console.WriteLine($"{item.name}: ${item.sum}");
			}

			Separate();

			Console.WriteLine("2. Департамент, в котором у сотрудника зарплата максимальна:");
			var departmentWhereEmployeeHasHighestSalary = employees.Where(employee => employee.Salary == employees.Max(e => e.Salary))
			                                                       .Join(departments, emp => emp.DepartmentId, department => department.Id,
			                                                             (employee, department) => new {name = department.Name});
			foreach(var item in departmentWhereEmployeeHasHighestSalary)
			{
				Console.WriteLine($"{item.name}");
			}

			//Департамент, в котором у сотрудника зарплата максимальна с указанием департамента и имени сотрудника
			//var departmentWhereEmployeeHasHighestSalary = employees.Where(employee => employee.Salary == employees.Max(e => e.Salary))
			//                                                       .Join(departments, emp => emp.DepartmentId, department => department.Id,
			//                                                             (employee, department) =>
			//	                                                             new {departmentName = department.Name, employeeName = employee.Name, salary = employee.Salary});
			//foreach(var item in departmentWhereEmployeeHasHighestSalary)
			//{
			//	Console.WriteLine($"{item.departmentName} {item.employeeName} - ${item.salary}");
			//}

			Separate();

			Console.WriteLine("3. Зарплаты руководителей департаментов (по убыванию):");
			var chiefSalary = employees.Where(e => e.ChiefId == 0).Select(e => e.Salary).OrderByDescending(i => i);
			foreach(var item in chiefSalary)
			{
				Console.WriteLine($"{item}");
			}

			//Зарплаты руководителей департаментов (по убыванию) с указанием департамента и имени руководителя
			//var chiefSalary = employees.Where(e => e.ChiefId == 0)
			//                           .Join(departments, emp => emp.DepartmentId, department => department.Id,
			//                                 (employee, department) => new {departmentName = department.Name, employeeName = employee.Name, salary = employee.Salary})
			//                           .OrderByDescending(i => i.salary);
			//foreach(var item in chiefSalary)
			//{
			//	Console.WriteLine($"{item.departmentName} {item.employeeName} - ${item.salary}");
			//}

			Separate();
		}

		private static void Separate() => Console.WriteLine(new string('-', 10));
	}
}
