using Microsoft.Extensions.Configuration;
using IfsApiTests.Config;

namespace IfsApiTests.Helpers;

public static class ConfigurationHelper
{
    private static ApiSettings? _settings;

    public static ApiSettings GetApiSettings()
    {
        if (_settings != null) return _settings;

        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .Build();

        _settings = new ApiSettings();
        config.GetSection("ApiSettings").Bind(_settings);
        return _settings;
    }
}
