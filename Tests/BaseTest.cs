using IfsApiTests.Clients;
using IfsApiTests.Helpers;

namespace IfsApiTests.Tests;

[SetUpFixture]
public class GlobalSetup
{
    [OneTimeSetUp]
    public void RunBeforeAllTests()
    {
        Console.WriteLine("=== IFS API Test Suite Starting ===");
    }

    [OneTimeTearDown]
    public void RunAfterAllTests()
    {
        Console.WriteLine("=== IFS API Test Suite Completed ===");
    }
}

public abstract class BaseTest
{
    protected ApiClient ApiClient { get; private set; } = null!;

    [SetUp]
    public void Setup()
    {
        var settings = ConfigurationHelper.GetApiSettings();
        ApiClient = new ApiClient(settings, enableLogging: true);
    }

    [TearDown]
    public void TearDown()
    {
        ApiClient?.Dispose();
    }
}
