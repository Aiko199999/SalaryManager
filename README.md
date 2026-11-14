# SalaryManager

A small payroll management web application built with C#, Blazor, and SQLite. The project is intended for a C# course final project and is arranged to work in Visual Studio Code.

## Requirements

- Visual Studio Code
- .NET 10 SDK
- VS Code extensions: C# Dev Kit and C#

Install the current .NET 10 SDK from [Microsoft](https://dotnet.microsoft.com/download/dotnet/10.0). The project targets .NET 10 LTS.

## Open the project

1. Extract the ZIP archive.
2. Open Visual Studio Code.
3. Select File -> Open Folder and choose the folder named `SalaryManager` that contains `SalaryManager.sln` and the `SalaryManager` and `SalaryManager.Tests` folders.
4. Open Terminal -> New Terminal.

## Run the application

Run these commands from the folder that contains `SalaryManager.sln`:

```bash
dotnet restore SalaryManager.sln
dotnet build SalaryManager.sln
dotnet run --project SalaryManager/SalaryManager.csproj --launch-profile SalaryManager
```

Open `http://localhost:5180` in your browser. Keep the terminal open while using the application. Stop the server with `Ctrl+C`.

To run the automated tests:

```bash
dotnet test SalaryManager.sln
```

You can also run **build**, **run**, and **test** from the VS Code Command Palette using **Tasks: Run Task**.

## What the application does

- Overview: employee count, monthly payroll, average payment, and pending payments.
- Employees: add, edit, search, activate, and deactivate employees.
- Payroll: calculate and save monthly payroll records.
- Payment history: review records and mark payments as paid.
- Reports: compare payroll totals by department and export a CSV file.
- A mock payment gateway returns a demo reference when a payroll record is saved; it does not transfer money.

The SQLite database is created automatically on the first run. Sample departments, employees, and payroll records are added once so the dashboard is not empty. To reset the sample data, stop the application and delete `SalaryManager/salarymanager.db`.

## Course topics used in the code

| Course topic | Where it is used |
|---|---|
| Records and immutability | `Models/PayrollModels.cs` contains separate draft, validated, and calculated payroll records. |
| Temporal coupling | `PayrollCalculator.Validate` must produce a `ValidatedPayroll` before `Calculate` can run. |
| Delegates and `Func` | `PayrollService` receives a `Func<decimal, string>` mock payment gateway through dependency injection. |
| Currying | `FunctionalExtensions.Curry` creates a reusable tax-rate function. |
| Composition and piping | `Compose` and `Pipe` are used in the payroll calculation. |
| Functional pipeline and `Aggregate` | `Pipeline` applies decimal operations in sequence with `Aggregate`. |
| Extension methods | `FunctionalExtensions.cs` contains reusable operations for payroll records. |
| LINQ | Filtering, grouping, ordering, `Sum`, `Average`, and projections are used in services and reports. |
| Lazy evaluation and `yield return` | `Filter` and `Transform` return deferred iterator sequences. The report materializes the result with `ToList`. |
| Dependency injection | The database factory, application services, options, state container, and mock gateway are registered in `Program.cs`. |
| Blazor and state management | Reusable components and the scoped `AppState` keep the interface in sync after data changes. Interactive Server uses SignalR for the live Blazor connection. |

## Notes about payroll figures

The tax rate is a demonstration value configured in `appsettings.json`. The application does not calculate statutory Kazakhstan payroll taxes, pension contributions, social contributions, or other mandatory payments. Do not use it for real payroll or real employee data.
