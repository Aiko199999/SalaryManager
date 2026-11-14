using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace SalaryManager.Tests;

public sealed class WebAppSmokeTests : IDisposable
{
    private readonly string contentRoot = Path.Combine(Path.GetTempPath(), "SalaryManagerTests", Guid.NewGuid().ToString("N"));
    private readonly TestFactory factory;

    public WebAppSmokeTests()
    {
        Directory.CreateDirectory(Path.Combine(contentRoot, "wwwroot"));
        factory = new TestFactory(contentRoot);
    }

    [Fact]
    public async Task HomePage_ReturnsHtmlAndShowsDashboard()
    {
        using var client = factory.CreateClient();
        using var response = await client.GetAsync("/");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("SalaryManager", html);
        Assert.Contains("Active employees", html);
        Assert.Contains("Recent payroll activity", html);
    }

    [Theory]
    [InlineData("/employees", "Employee directory")]
    [InlineData("/payroll", "Calculate payroll")]
    [InlineData("/history", "All payroll records")]
    [InlineData("/reports", "Department report")]
    public async Task MainPages_ReturnHtml(string path, string expectedText)
    {
        using var client = factory.CreateClient();
        using var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains(expectedText, html);
    }

    public void Dispose()
    {
        factory.Dispose();
        if (Directory.Exists(contentRoot))
            Directory.Delete(contentRoot, recursive: true);
    }

    private sealed class TestFactory(string testContentRoot) : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development");
            builder.UseContentRoot(testContentRoot);
        }
    }
}
