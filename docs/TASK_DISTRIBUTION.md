# Team Work Distribution

## Team structure

The project is completed by two students. The workload is divided into two equal feature areas.

## Aiym — Employee Management, Database, and User Interface

**Main responsibility:** employee records, database persistence, dashboard, and the shared visual layout.

### Files owned

| File | Responsibility |
|---|---|
| `SalaryManager/Models/Employee.cs` | Employee data model and validation rules |
| `SalaryManager/Models/Department.cs` | Department data model |
| `SalaryManager/Data/SalaryDbContext.cs` | EF Core database configuration and relationships |
| `SalaryManager/Data/DatabaseInitializer.cs` | Database creation and initial sample data |
| `SalaryManager/Services/IEmployeeService.cs` | Employee service contract |
| `SalaryManager/Services/EmployeeService.cs` | Employee create, read, update, search, and activation operations |
| `SalaryManager/Components/Pages/Employees.razor` | Employee list, search, and employee forms |
| `SalaryManager/Components/Pages/Home.razor` | Dashboard cards and summary information |
| `SalaryManager/Components/Layout/MainLayout.razor` | Main application layout and navigation |
| `SalaryManager/Components/Shared/MetricCard.razor` | Reusable dashboard metric component |
| `SalaryManager/Components/Shared/StatusTag.razor` | Reusable status component |
| `SalaryManager/wwwroot/app.css` | Responsive layout, colours, spacing, and component styling |
| `SalaryManager/SalaryManager.Tests/DatabaseIntegrationTests.cs` | SQLite persistence and database integration tests |

### Required work

1. Implement and verify employee and department data storage.
2. Implement employee creation, editing, searching, and activation/deactivation.
3. Build the dashboard and reusable UI components.
4. Make the pages usable at desktop and mobile widths.
5. Write and run the database integration tests.
6. Explain the database schema, EF Core, dependency injection, reusable components, and responsive design during the defense.

## Baizhana — Payroll, Functional C# Concepts, Reports, and Application Tests

**Main responsibility:** payroll workflow and calculations, course-specific functional programming examples, payment history, reports, and automated application tests.

### Files owned

| File | Responsibility |
|---|---|
| `SalaryManager/Models/PayrollEntry.cs` | Saved payroll entry and payment status |
| `SalaryManager/Models/PayrollModels.cs` | Immutable payroll workflow records |
| `SalaryManager/Services/IPayrollService.cs` | Payroll service contract |
| `SalaryManager/Services/PayrollService.cs` | Payroll preview, saving, history, payment status, and reporting data |
| `SalaryManager/Services/PayrollCalculator.cs` | Payroll validation and calculation workflow |
| `SalaryManager/Services/PayrollOptions.cs` | Configurable demonstration calculation settings |
| `SalaryManager/Services/AppState.cs` | Shared application state notifications |
| `SalaryManager/Functional/FunctionalExtensions.cs` | Curry, Compose, Pipe, Pipeline, Aggregate, LINQ helpers, lazy Filter and Transform |
| `SalaryManager/Components/Pages/Payroll.razor` | Payroll input, validation, preview, and save actions |
| `SalaryManager/Components/Pages/History.razor` | Payment history and payment status actions |
| `SalaryManager/Components/Pages/Reports.razor` | Department reports and CSV export |
| `SalaryManager/Components/Pages/Error.razor` | User-facing error page |
| `SalaryManager/SalaryManager.Tests/PayrollCalculatorTests.cs` | Payroll calculation and validation unit tests |
| `SalaryManager/SalaryManager.Tests/FunctionalExtensionsTests.cs` | Tests for functional helpers and lazy iterators |
| `SalaryManager/SalaryManager.Tests/WebAppSmokeTests.cs` | HTTP smoke test for application startup and response |

### Required work

1. Implement and verify payroll validation, calculation, and saving.
2. Use immutable records to represent the stages of the payroll workflow.
3. Demonstrate temporal-coupling prevention by requiring a validated payroll record before calculation.
4. Implement and explain delegates, `Func`, currying, composition, piping, and `Aggregate`.
5. Implement extension methods, LINQ queries, lazy `Filter` and `Transform` iterators using `yield return`.
6. Build payment history, reports, and CSV export.
7. Write and run payroll, functional-helper, and application smoke tests.