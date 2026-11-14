using SalaryManager.Models;

namespace SalaryManager.Functional;

public static class FunctionalExtensions
{
    public static Func<TInput, TResult> Compose<TInput, TMiddle, TResult>(
        this Func<TInput, TMiddle> first,
        Func<TMiddle, TResult> second)
        => value => second(first(value));

    public static TResult Pipe<TInput, TResult>(this TInput value, Func<TInput, TResult> operation)
        => operation(value);

    public static Func<TFirst, Func<TSecond, TResult>> Curry<TFirst, TSecond, TResult>(
        this Func<TFirst, TSecond, TResult> function)
        => first => second => function(first, second);

    public static decimal Pipeline(this decimal value, params Func<decimal, decimal>[] steps)
        => steps.Aggregate(value, (current, step) => step(current));

    public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        foreach (var item in source)
        {
            if (predicate(item))
                yield return item;
        }
    }

    public static IEnumerable<TResult> Transform<T, TResult>(
        this IEnumerable<T> source,
        Func<T, TResult> selector)
    {
        foreach (var item in source)
            yield return selector(item);
    }

    public static IEnumerable<PayrollEntry> ForDepartment(
        this IEnumerable<PayrollEntry> source,
        int departmentId)
        => source.Filter(entry => entry.Employee?.DepartmentId == departmentId);

    public static decimal TotalNetSalary(this IEnumerable<PayrollEntry> source)
        => source.Sum(entry => entry.NetSalary);

    public static decimal AverageNetSalary(this IEnumerable<PayrollEntry> source)
    {
        var values = source.Select(entry => entry.NetSalary).ToList();
        return values.Count == 0 ? 0 : values.Average();
    }

    public static IEnumerable<PayrollEntry> LargestPayments(
        this IEnumerable<PayrollEntry> source,
        int count)
        => source.OrderByDescending(entry => entry.NetSalary).Take(Math.Max(0, count));
}
