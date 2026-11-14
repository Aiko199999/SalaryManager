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

//builder services...