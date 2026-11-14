using SalaryManager.Functional;
using SalaryManager.Models;
using Xunit;

namespace SalaryManager.Tests;

public sealed class FunctionalExtensionsTests
{
    [Fact]
    public void LargestPayments_ReturnsTheHighestNetAmounts()
    {
        var entries = new[]
        {
            new PayrollEntry { NetSalary = 100m },
            new PayrollEntry { NetSalary = 350m },
            new PayrollEntry { NetSalary = 250m }
        };

        Assert.Equal(new[] { 350m, 250m }, entries.LargestPayments(2).Select(item => item.NetSalary));
    }

    [Fact]
    public void TotalNetSalary_ReturnsTheSumOfPayments()
    {
        var entries = new[]
        {
            new PayrollEntry { NetSalary = 100m },
            new PayrollEntry { NetSalary = 250m }
        };

        Assert.Equal(350m, entries.TotalNetSalary());
    }
}
