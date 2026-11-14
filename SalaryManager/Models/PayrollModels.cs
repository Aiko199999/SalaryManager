namespace SalaryManager.Models;

public sealed record PayrollDraft(
    int EmployeeId,
    decimal BaseSalary,
    decimal Bonus,
    decimal Deductions,
    decimal TaxRate);

public sealed record ValidatedPayroll(
    int EmployeeId,
    decimal BaseSalary,
    decimal Bonus,
    decimal Deductions,
    decimal GrossSalary,
    decimal TaxRate);

public sealed record PayrollCalculation(
    decimal GrossSalary,
    decimal TaxableSalary,
    decimal Tax,
    decimal NetSalary);

public sealed record PayrollSaveResult(PayrollEntry Entry, string Receipt);

public sealed record DashboardSummary(
    int ActiveEmployees,
    decimal MonthlyPayroll,
    decimal AverageNetSalary,
    int PendingPayments,
    IReadOnlyList<PayrollEntry> RecentEntries);

public sealed record DepartmentSummary(
    string DepartmentName,
    int EmployeeCount,
    decimal TotalPayroll,
    decimal AverageNetSalary);
