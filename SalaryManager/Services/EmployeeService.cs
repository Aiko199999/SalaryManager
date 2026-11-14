using Microsoft.EntityFrameworkCore;
using SalaryManager.Data;
using SalaryManager.Models;

namespace SalaryManager.Services;

public sealed class EmployeeService(
    IDbContextFactory<SalaryDbContext> contextFactory,
    AppState appState) : IEmployeeService
{
    public async Task<IReadOnlyList<Employee>> GetAllAsync()
    {
        await using var db = await contextFactory.CreateDbContextAsync();
        return await db.Employees
            .AsNoTracking()
            .Include(employee => employee.Department)
            .OrderBy(employee => employee.FullName)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Employee>> GetActiveAsync()
    {
        await using var db = await contextFactory.CreateDbContextAsync();
        return await db.Employees
            .AsNoTracking()
            .Where(employee => employee.IsActive)
            .OrderBy(employee => employee.FullName)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Department>> GetDepartmentsAsync()
    {
        await using var db = await contextFactory.CreateDbContextAsync();
        return await db.Departments.AsNoTracking().OrderBy(item => item.Name).ToListAsync();
    }

    public async Task SaveAsync(Employee employee)
    {
        if (string.IsNullOrWhiteSpace(employee.FullName) || string.IsNullOrWhiteSpace(employee.JobTitle))
            throw new ArgumentException("Enter the employee name and job title.");
        if (employee.BaseSalary <= 0)
            throw new ArgumentException("Base salary must be greater than zero.");

        await using var db = await contextFactory.CreateDbContextAsync();
        var departmentExists = await db.Departments.AnyAsync(item => item.Id == employee.DepartmentId);
        if (!departmentExists)
            throw new ArgumentException("Select a valid department.");

        if (employee.Id == 0)
        {
            db.Employees.Add(new Employee
            {
                FullName = employee.FullName.Trim(),
                JobTitle = employee.JobTitle.Trim(),
                DepartmentId = employee.DepartmentId,
                BaseSalary = employee.BaseSalary,
                IsActive = true
            });
        }
        else
        {
            var existing = await db.Employees.FindAsync(employee.Id)
                ?? throw new InvalidOperationException("The employee could not be found.");
            existing.FullName = employee.FullName.Trim();
            existing.JobTitle = employee.JobTitle.Trim();
            existing.DepartmentId = employee.DepartmentId;
            existing.BaseSalary = employee.BaseSalary;
        }

        await db.SaveChangesAsync();
        appState.NotifyChanged();
    }

    public async Task SetActiveAsync(int employeeId, bool isActive)
    {
        await using var db = await contextFactory.CreateDbContextAsync();
        var employee = await db.Employees.FindAsync(employeeId)
            ?? throw new InvalidOperationException("The employee could not be found.");
        employee.IsActive = isActive;
        await db.SaveChangesAsync();
        appState.NotifyChanged();
    }
}
