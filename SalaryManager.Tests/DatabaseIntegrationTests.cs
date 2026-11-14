using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SalaryManager.Data;
using SalaryManager.Models;
using Xunit;

namespace SalaryManager.Tests;

public sealed class DatabaseIntegrationTests
{
    [Fact]
    public async Task EmployeeAndPayrollCanBeSavedAndLoadedWithTheirRelationships()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<SalaryDbContext>()
            .UseSqlite(connection)
            .Options;

        await using (var db = new SalaryDbContext(options))
        {
            await db.Database.EnsureCreatedAsync();
            var department = new Department { Name = "Finance" };
            db.Departments.Add(department);
            await db.SaveChangesAsync();

            var employee = new Employee
            {
                FullName = "Taylor Morgan",
                JobTitle = "Accountant",
                DepartmentId = department.Id,
                BaseSalary = 400000m
            };
            db.Employees.Add(employee);
            await db.SaveChangesAsync();

            db.PayrollEntries.Add(new PayrollEntry
            {
                EmployeeId = employee.Id,
                Period = new DateTime(2026, 11, 1),
                BaseSalary = 400000m,
                GrossSalary = 400000m,
                TaxableSalary = 400000m,
                Tax = 40000m,
                NetSalary = 360000m
            });
            await db.SaveChangesAsync();
        }

        await using (var db = new SalaryDbContext(options))
        {
            var savedEmployee = await db.Employees
                .Include(employee => employee.Department)
                .SingleAsync();
            var savedPayroll = await db.PayrollEntries.SingleAsync();

            Assert.Equal("Taylor Morgan", savedEmployee.FullName);
            Assert.Equal("Finance", savedEmployee.Department!.Name);
            Assert.Equal(360000m, savedPayroll.NetSalary);
        }
    }
}
