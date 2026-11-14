using SalaryManager.Models;

namespace SalaryManager.Services;

public interface IEmployeeService
{
    Task<IReadOnlyList<Employee>> GetAllAsync();
    Task<IReadOnlyList<Employee>> GetActiveAsync();
    Task<IReadOnlyList<Department>> GetDepartmentsAsync();
    Task SaveAsync(Employee employee);
    Task SetActiveAsync(int employeeId, bool isActive);
}
