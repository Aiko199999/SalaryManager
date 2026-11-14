using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalaryManager.Data;
using SalaryManager.Functional;
using SalaryManager.Models;

namespace SalaryManager.Services;

public sealed class PayrollService(
    IDbContextFactory<SalaryDbContext> contextFactory,
    IOptions<PayrollOptions> payrollOptions,
    Func<decimal, string> paymentGateway,
    AppState appState) : IPayrollService
{
    public async Task<PayrollCalculation> PreviewAsync(int employeeId, decimal bonus, decimal deductions)
    {
        await using var db = await contextFactory.CreateDbContextAsync();
        var employee = await db.Employees.AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == employeeId && item.IsActive)
            ?? throw new ArgumentException("The selected employee is not available.");

        var draft = new PayrollDraft(employee.Id, employee.BaseSalary, bonus, deductions, payrollOptions.Value.TaxRate);
        var validated = PayrollCalculator.Validate(draft);
        return PayrollCalculator.Calculate(validated);
    }

    public async Task<PayrollSaveResult> SaveAsync(
        int employeeId,
        DateTime period,
        decimal bonus,
        decimal deductions)
    {
        await using var db = await contextFactory.CreateDbContextAsync();
        var employee = await db.Employees
            .FirstOrDefaultAsync(item => item.Id == employeeId && item.IsActive)
            ?? throw new ArgumentException("The selected employee is not available.");

        var draft = new PayrollDraft(employee.Id, employee.BaseSalary, bonus, deductions, payrollOptions.Value.TaxRate);
        var validated = PayrollCalculator.Validate(draft);
        var result = PayrollCalculator.Calculate(validated);

        var paymentReference = paymentGateway(result.NetSalary);
        var entry = new PayrollEntry
        {
            EmployeeId = employee.Id,
            Period = new DateTime(period.Year, period.Month, 1),
            BaseSalary = employee.BaseSalary,
            Bonus = bonus,
            Deductions = deductions,
            GrossSalary = result.GrossSalary,
            TaxableSalary = result.TaxableSalary,
            Tax = result.Tax,
            NetSalary = result.NetSalary,
            PaymentReference = paymentReference,
            IsPaid = false
        };

        db.PayrollEntries.Add(entry);
        await db.SaveChangesAsync();
        appState.NotifyChanged();

        var receipt = string.Join(Environment.NewLine,
            $"Payroll receipt — {employee.FullName}",
            $"Gross salary: {result.GrossSalary:N2} KZT",
            $"Taxable salary: {result.TaxableSalary:N2} KZT",
            $"Demonstration tax: {result.Tax:N2} KZT",
            $"Net salary: {result.NetSalary:N2} KZT",
            $"Demo gateway reference: {paymentReference}");

        return new PayrollSaveResult(entry, receipt);
    }

    public async Task<IReadOnlyList<PayrollEntry>> GetHistoryAsync()
    {
        await using var db = await contextFactory.CreateDbContextAsync();
        return await db.PayrollEntries
            .AsNoTracking()
            .Include(entry => entry.Employee)
            .ThenInclude(employee => employee!.Department)
            .OrderByDescending(entry => entry.Period)
            .ThenByDescending(entry => entry.Id)
            .ToListAsync();
    }

    public async Task MarkPaidAsync(int payrollId)
    {
        await using var db = await contextFactory.CreateDbContextAsync();
        var entry = await db.PayrollEntries.FindAsync(payrollId)
            ?? throw new InvalidOperationException("The payroll record could not be found.");
        entry.IsPaid = true;
        await db.SaveChangesAsync();
        appState.NotifyChanged();
    }

    public async Task<DashboardSummary> GetDashboardAsync()
    {
        await using var db = await contextFactory.CreateDbContextAsync();
        var activeEmployees = await db.Employees.CountAsync(employee => employee.IsActive);
        var month = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        var entries = await db.PayrollEntries
            .AsNoTracking()
            .Where(entry => entry.Period == month)
            .OrderByDescending(entry => entry.Id)
            .ToListAsync();
        var recent = await db.PayrollEntries
            .AsNoTracking()
            .Include(entry => entry.Employee)
            .OrderByDescending(entry => entry.Id)
            .Take(5)
            .ToListAsync();

        return new DashboardSummary(
            activeEmployees,
            entries.TotalNetSalary(),
            entries.AverageNetSalary(),
            entries.Count(entry => !entry.IsPaid),
            recent);
    }

    public async Task<IReadOnlyList<DepartmentSummary>> GetDepartmentReportAsync()
    {
        await using var db = await contextFactory.CreateDbContextAsync();
        var entries = await db.PayrollEntries
            .AsNoTracking()
            .Include(entry => entry.Employee)
            .ThenInclude(employee => employee!.Department)
            .ToListAsync();

        var reportRows = entries
            .Filter(entry => entry.NetSalary >= 0)
            .Transform(entry => new
            {
                entry.EmployeeId,
                Department = entry.Employee?.Department?.Name ?? "Unassigned",
                entry.NetSalary
            })
            .ToList();

        return reportRows
            .GroupBy(entry => entry.Department)
            .Select(group => new DepartmentSummary(
                group.Key,
                group.Select(entry => entry.EmployeeId).Distinct().Count(),
                group.Sum(entry => entry.NetSalary),
                group.Average(entry => entry.NetSalary)))
            .OrderByDescending(summary => summary.TotalPayroll)
            .ToList();
    }
}
