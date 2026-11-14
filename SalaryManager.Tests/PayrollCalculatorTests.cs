using SalaryManager.Functional;
using SalaryManager.Models;
using SalaryManager.Services;
using Xunit;

namespace SalaryManager.Tests;

public sealed class PayrollCalculatorTests
{
    [Fact]
    public void Calculate_ReturnsExpectedAmounts()
    {
        var result = PayrollCalculator.Calculate(
            new PayrollDraft(1, 500000m, 50000m, 10000m, 0.10m));

        Assert.Equal(550000m, result.GrossSalary);
        Assert.Equal(540000m, result.TaxableSalary);
        Assert.Equal(54000m, result.Tax);
        Assert.Equal(486000m, result.NetSalary);
    }

    [Fact]
    public void Validate_RejectsNegativeBonus()
    {
        var draft = new PayrollDraft(1, 300000m, -1m, 0m, 0.10m);
        Assert.Throws<ArgumentException>(() => PayrollCalculator.Validate(draft));
    }

    [Fact]
    public void Validate_RejectsDeductionsAboveGrossSalary()
    {
        var draft = new PayrollDraft(1, 300000m, 0m, 300001m, 0.10m);
        Assert.Throws<ArgumentException>(() => PayrollCalculator.Validate(draft));
    }

    [Fact]
    public void Validate_RejectsTaxRateOutsideAllowedRange()
    {
        var draft = new PayrollDraft(1, 300000m, 0m, 0m, 1.1m);
        Assert.Throws<ArgumentException>(() => PayrollCalculator.Validate(draft));
    }

    [Fact]
    public void Pipeline_AppliesStepsInOrder()
    {
        var result = 100m.Pipeline(amount => amount + 20m, amount => amount * 2m);
        Assert.Equal(240m, result);
    }

    [Fact]
    public void Curry_CreatesAReusableTaxFunction()
    {
        Func<decimal, decimal, decimal> multiply = (amount, rate) => amount * rate;
        var tenPercentOf = multiply.Curry()(500m);
        Assert.Equal(50m, tenPercentOf(0.10m));
    }

    [Fact]
    public void Compose_RunsTheFirstFunctionBeforeTheSecond()
    {
        Func<int, int> addTwo = value => value + 2;
        Func<int, int> multiplyByThree = value => value * 3;
        Assert.Equal(15, addTwo.Compose(multiplyByThree)(3));
    }

    [Fact]
    public void LazyFilterAndTransform_RunWhenTheSequenceIsEnumerated()
    {
        var visited = new List<int>();
        var query = new[] { 1, 2, 3 }
            .Filter(value =>
            {
                visited.Add(value);
                return value > 1;
            })
            .Transform(value => value * 10);

        Assert.Empty(visited);
        Assert.Equal(new[] { 20, 30 }, query.ToList());
        Assert.Equal(new[] { 1, 2, 3 }, visited);
    }

    [Fact]
    public void AverageNetSalary_ReturnsZeroForAnEmptySequence()
    {
        Assert.Equal(0m, Array.Empty<PayrollEntry>().AverageNetSalary());
    }
}
