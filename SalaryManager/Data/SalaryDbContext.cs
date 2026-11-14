using Microsoft.EntityFrameworkCore;
using SalaryManager.Models;

namespace SalaryManager.Data;

public sealed class SalaryDbContext(DbContextOptions<SalaryDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<PayrollEntry> PayrollEntries => Set<PayrollEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>()
            .HasIndex(department => department.Name)
            .IsUnique();

        modelBuilder.Entity<Employee>()
            .HasOne(employee => employee.Department)
            .WithMany(department => department.Employees)
            .HasForeignKey(employee => employee.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PayrollEntry>()
            .HasOne(entry => entry.Employee)
            .WithMany(employee => employee.PayrollEntries)
            .HasForeignKey(entry => entry.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PayrollEntry>()
            .HasIndex(entry => new { entry.EmployeeId, entry.Period });
    }
}
