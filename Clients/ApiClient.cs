using RestSharp;
using IfsApiTests.Config;

namespace IfsApiTests.Clients;

public class ApiClient : IDisposable
{
    private readonly RestClient _client;
    private readonly bool _enableLogging;

    public ApiClient(ApiSettings settings, bool enableLogging = true)
    {
        var options = new RestClientOptions(settings.BaseUrl)
        {
            Timeout = TimeSpan.FromSeconds(settings.TimeoutSeconds)
        };
        _client = new RestClient(options);
        _enableLogging = enableLogging;
    }

    public async Task<RestResponse<T>> ExecuteAsync<T>(RestRequest request)
    {
        LogRequest(request);
        var response = await _client.ExecuteAsync<T>(request);
        LogResponse(response);
        return response;
    }

    public async Task<RestResponse> ExecuteAsync(RestRequest request)
    {
        LogRequest(request);
        var response = await _client.ExecuteAsync(request);
        LogResponse(response);
        return response;
    }

    private void LogRequest(RestRequest request)
    {
        if (!_enableLogging) return;
        Console.WriteLine($"[REQUEST] {request.Method} /{request.Resource}");
    }

    private void LogResponse(RestResponseBase response)
    {
        if (!_enableLogging) return;
        Console.WriteLine($"[RESPONSE] Status: {(int)response.StatusCode} {response.StatusCode}");
        if (!string.IsNullOrEmpty(response.Content))
        {
            var preview = response.Content.Length > 200
                ? response.Content[..200] + "..."
                : response.Content;
            Console.WriteLine($"[RESPONSE BODY] {preview}");
        }
        Console.WriteLine();
    }

    public void Dispose() => _client.Dispose();
}