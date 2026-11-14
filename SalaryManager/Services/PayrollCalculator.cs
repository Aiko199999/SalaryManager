using SalaryManager.Functional;
using SalaryManager.Models;

namespace SalaryManager.Services;

public static class PayrollCalculator
{
    public static ValidatedPayroll Validate(PayrollDraft draft)
    {
        if (draft.EmployeeId <= 0)
            throw new ArgumentException("Select an employee.");
        if (draft.BaseSalary <= 0)
            throw new ArgumentException("Base salary must be greater than zero.");
        if (draft.Bonus < 0 || draft.Deductions < 0)
            throw new ArgumentException("Bonus and deductions cannot be negative.");
        if (draft.Deductions > draft.BaseSalary + draft.Bonus)
            throw new ArgumentException("Deductions cannot exceed gross salary.");
        if (draft.TaxRate is < 0 or > 1)
            throw new ArgumentException("Tax rate must be between 0% and 100%.");

        var gross = draft.BaseSalary + draft.Bonus;
        return new ValidatedPayroll(
            draft.EmployeeId,
            draft.BaseSalary,
            draft.Bonus,
            draft.Deductions,
            gross,
            draft.TaxRate);
    }

    public static PayrollCalculation Calculate(ValidatedPayroll payroll)
    {
        var taxable = payroll.GrossSalary.Pipeline(
            amount => amount - payroll.Deductions,
            amount => Math.Max(0, amount));

        Func<decimal, decimal, decimal> multiply = (amount, rate) => amount * rate;
        var rawTaxForRate = multiply.Curry()(taxable);
        var roundTax = new Func<decimal, decimal>(amount => Math.Round(amount, 2, MidpointRounding.AwayFromZero));
        var roundedTaxForRate = rawTaxForRate.Compose(roundTax);
        var tax = payroll.TaxRate.Pipe(roundedTaxForRate);
        var net = (taxable - tax).Pipe(amount => Math.Round(amount, 2, MidpointRounding.AwayFromZero));

        return new PayrollCalculation(payroll.GrossSalary, taxable, tax, net);
    }

    public static PayrollCalculation Calculate(PayrollDraft draft)
        => draft.Pipe(Validate).Pipe(Calculate);
}
