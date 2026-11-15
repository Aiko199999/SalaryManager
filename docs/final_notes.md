# Final Project Notes

## Project summary

**Topic:** SalaryManager — Payroll Management System

**Aim:** Build a small web application that stores employee information, calculates sample payroll, tracks payment status, and summarizes payroll by department.

**Practical value:** A small organization can use the application as a simple demonstration of how employee and payroll records are organized. The tax formula is intentionally illustrative and is not a legal payroll calculation.

## Architecture

The application uses a simple layered structure inside one Blazor web project:

- **Components:** pages, layout, and reusable UI components.
- **Services:** employee and payroll operations, validation, and application state.
- **Data:** EF Core context and database initialization.
- **Models:** database entities and immutable payroll workflow records.
- **Functional:** reusable delegates, pipelines, LINQ helpers, and lazy iterators.

SQLite stores the data. Services receive dependencies through .NET dependency injection. `IDbContextFactory` creates a short-lived database context for each operation. Read-only queries use `AsNoTracking`.

## Short technology comparison

| Option | Strength | Limitation for this project |
|---|---|---|
| Blazor Server / Interactive Server | C# on the server, quick UI updates, shared .NET code | Requires an active server connection |
| Blazor WebAssembly | Runs client-side in the browser | More client setup and API work for a small database-backed project |
| ASP.NET Core MVC | Clear request/response model and mature patterns | More page navigation and JavaScript for interactive forms |
| SQLite | Simple local relational database with no separate server | Intended for a small academic application, not a high-volume payroll platform |

## Course-topics

- **Temporal coupling:** a `PayrollDraft` is validated into `ValidatedPayroll`; calculation accepts only the validated record. The stages cannot be called in the wrong order by passing the original draft.
- **Currying:** `Curry` turns a two-argument multiplication function into a function with the tax base already supplied.
- **Composition:** the raw tax function is composed with a rounding function.
- **Pipelining:** deductions and the non-negative check are applied in order through `Pipeline` and `Aggregate`.
- **LINQ:** reports group entries by department and calculate sums and averages.
- **Lazy evaluation:** `Filter` and `Transform` use `yield return`; their work starts when the sequence is enumerated and materialized with `ToList`.
- **Dependency injection:** the payroll service receives a database factory, options, state container, and `Func<decimal, string>` mock payment gateway. It returns a simulation reference and does not transfer money.

## Verification checklist

Run these commands from the solution folder:

```bash
dotnet restore SalaryManager.sln
dotnet build SalaryManager.sln
dotnet test SalaryManager.sln
```

## Rubric map

| No. | Criterion | Evidence in this project |
|---:|---|---|
| 1 | Code structure and design | UI, services, data access, models, and functional helpers have separate responsibilities. |
| 2 | Component reusability | `MetricCard` and `StatusTag` are shared Blazor components with parameters. |
| 3 | Cross-platform compatibility | The application is browser-based and runs on the cross-platform .NET runtime. |
| 4 | State management | The scoped `AppState` notifies active components after data changes. |
| 5 | UI design | Consistent spacing, typography, colours, tables, and cards are defined in `wwwroot/app.css`. |
| 6 | User experience | Navigation, search, validation, calculation preview, and clear payment states are provided. |
| 7 | Responsiveness | CSS media queries adapt the layout and navigation for smaller screens. |
| 8 | Core functionality | Employee records, payroll calculation, payment history, reporting, and CSV export are implemented. |
| 9 | Advanced features | Interactive Server uses SignalR for the live UI connection; the app also provides CSV export and shared state updates. |
| 10 | Error handling and resilience | Payroll input is validated before calculation; service errors produce user-facing messages. |
| 11 | Efficiency | Read queries use `AsNoTracking`; each service operation uses a short-lived context. |
| 12 | Scalability | Services are separated from UI components, and database indexes support common lookups. |
| 13 | Resource utilization | The app avoids keeping a database context open for the lifetime of a Blazor circuit. |
| 14 | Testing coverage | Automated unit, SQLite integration, and HTTP application smoke tests are included. |
| 15 | Quality assurance | A manual browser and mobile-width test checklist is included; complete it before the defense. |
| 16 | Documentation | README and this defense/checklist document explain setup, scope, architecture, and usage. |
| 17 | Innovation | Immutable workflow stages and functional composition make the payroll calculation order explicit. |
| 18 | Blazor/C#/.NET use | Interactive Server, reusable components, records, delegates, LINQ, iterators, and dependency injection are used. |
| 19 | Professionalism | The UI has consistent styling, meaningful validation, saved history, and clear project boundaries. |
| 20 | Presentation | The suggested defense flow covers the aim, comparison, architecture, live demonstration, and course topics. |