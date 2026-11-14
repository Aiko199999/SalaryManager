namespace SalaryManager.Services;

public sealed class AppState
{
    public event Action? Changed;

    public void NotifyChanged() => Changed?.Invoke();
}
