using Microsoft.EntityFrameworkCore;
using SalaryManager.Models;
using SalaryManager.Services;

namespace SalaryManager.Data;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();
        var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<SalaryDbContext>>();
        var options = scope.ServiceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<PayrollOptions>>();
        await using var db = await factory.CreateDbContextAsync();

        await db.Database.EnsureCreatedAsync();

        if (!await db.Departments.AnyAsync())
        {
            db.Departments.AddRange(
                new Department { Name = "Administration" },
                new Department { Name = "Finance" },
                new Department { Name = "Technology" });
            await db.SaveChangesAsync();
        }

        if (!await db.Employees.AnyAsync())
        {
            var departments = await db.Departments.ToDictionaryAsync(item => item.Name, item => item.Id);
            db.Employees.AddRange(
                new Employee { FullName = "Olivia Parker", JobTitle = "Office Manager", DepartmentId = departments["Administration"], BaseSalary = 420000 },
                new Employee { FullName = "Daniel Brooks", JobTitle = "Accountant", DepartmentId = departments["Finance"], BaseSalary = 500000 },
                new Employee { FullName = "Emma Wilson", JobTitle = "Software Developer", DepartmentId = departments["Technology"], BaseSalary = 650000 },
                new Employee { FullName = "Noah Reed", JobTitle = "Junior Developer", DepartmentId = departments["Technology"], BaseSalary = 380000 });
            await db.SaveChangesAsync();
        }

        if (!await db.PayrollEntries.AnyAsync())
        {
            var employees = await db.Employees.OrderBy(item => item.Id).ToListAsync();
            var period = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            foreach (var employee in employees.Take(3))
            {
                var draft = new PayrollDraft(employee.Id, employee.BaseSalary, 0, 0, options.Value.TaxRate);
                var result = PayrollCalculator.Calculate(draft);
                db.PayrollEntries.Add(new PayrollEntry
                {
                    EmployeeId = employee.Id,
                    Period = period,
                    BaseSalary = employee.BaseSalary,
                    GrossSalary = result.GrossSalary,
                    TaxableSalary = result.TaxableSalary,
                    Tax = result.Tax,
                    NetSalary = result.NetSalary,
                    IsPaid = employee.Id == employees[0].Id
                });
            }
            await db.SaveChangesAsync();
        }
    }
}
