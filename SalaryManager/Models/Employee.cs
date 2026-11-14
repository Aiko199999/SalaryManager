using System.ComponentModel.DataAnnotations;

namespace SalaryManager.Models;

public sealed class Employee
{
    public int Id { get; set; }

    [Required, StringLength(80, MinimumLength = 2)]
    public string FullName { get; set; } = string.Empty;

    [Required, StringLength(60)]
    public string JobTitle { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int DepartmentId { get; set; }

    [Range(typeof(decimal), "1", "100000000")]
    public decimal BaseSalary { get; set; }

    public bool IsActive { get; set; } = true;
    public Department? Department { get; set; }
    public List<PayrollEntry> PayrollEntries { get; set; } = [];
}
