using Microsoft.EntityFrameworkCore;
using SalaryManager.Components;
using SalaryManager.Data;
using SalaryManager.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var databasePath = Path.Combine(builder.Environment.ContentRootPath, "salarymanager.db");
builder.Services.AddDbContextFactory<SalaryDbContext>(options =>
    options.UseSqlite($"Data Source={databasePath}"));

builder.Services.Configure<PayrollOptions>(builder.Configuration.GetSection("Payroll"));
builder.Services.AddScoped<AppState>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IPayrollService, PayrollService>();
builder.Services.AddScoped<Func<decimal, string>>(services =>
    amount => "SIM-" + Guid.NewGuid().ToString("N")[..8].ToUpperInvariant());

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
    app.UseHttpsRedirection();
}
app.UseStaticFiles();
app.UseAntiforgery();

await DatabaseInitializer.InitializeAsync(app.Services);

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

public partial class Program { }
