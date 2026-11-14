using SalaryManager.Models;

namespace SalaryManager.Services;

public interface IPayrollService
{
    Task<PayrollCalculation> PreviewAsync(int employeeId, decimal bonus, decimal deductions);
    Task<PayrollSaveResult> SaveAsync(int employeeId, DateTime period, decimal bonus, decimal deductions);
    Task<IReadOnlyList<PayrollEntry>> GetHistoryAsync();
    Task MarkPaidAsync(int payrollId);
    Task<DashboardSummary> GetDashboardAsync();
    Task<IReadOnlyList<DepartmentSummary>> GetDepartmentReportAsync();
}
