using RestSharp;
using IfsApiTests.Config;
using System.Net;

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

    #region Generic Methods
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
    #endregion

    #region Shorthand Methods
    public async Task<RestResponse<T>> GetAsync<T>(string resource)
    {
        var request = new RestRequest(resource, Method.Get);
        return await ExecuteAsync<T>(request);
    }

    public async Task<RestResponse<T>> PostAsync<T>(string resource, object body)
    {
        var request = new RestRequest(resource, Method.Post);
        request.AddJsonBody(body);
        return await ExecuteAsync<T>(request);
    }

    public async Task<RestResponse<T>> PutAsync<T>(string resource, object body)
    {
        var request = new RestRequest(resource, Method.Put);
        request.AddJsonBody(body);
        return await ExecuteAsync<T>(request);
    }

    public async Task<RestResponse> DeleteAsync(string resource)
    {
        var request = new RestRequest(resource, Method.Delete);
        return await ExecuteAsync(request);
    }
    #endregion

    #region Logging
    private void LogRequest(RestRequest request)
    {
        if (!_enableLogging) return;
        Console.WriteLine($"[REQUEST] {request.Method} {request.Resource}");
    }

    private void LogResponse(RestResponseBase response)
    {
        if (!_enableLogging) return;
        Console.WriteLine($"[RESPONSE] Status: {(int)response.StatusCode} {response.StatusCode}");
        
        if (!string.IsNullOrEmpty(response.Content))
        {
            var preview = response.Content.Length > 500
                ? response.Content[..500] + "..."
                : response.Content;
            Console.WriteLine($"[RESPONSE BODY] {preview}");
        }
        Console.WriteLine(new string('-', 30));
    }
    #endregion

    public void Dispose() => _client.Dispose();
}
