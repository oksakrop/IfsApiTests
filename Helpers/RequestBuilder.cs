using RestSharp;
using Newtonsoft.Json;

namespace IfsApiTests.Helpers;

public static class RequestBuilder
{
    public static RestRequest Get(string resource) =>
        new RestRequest(resource, Method.Get);

    public static RestRequest Post<T>(string resource, T body) where T : class
    {
        var request = new RestRequest(resource, Method.Post);
        request.AddJsonBody(JsonConvert.SerializeObject(body));
        return request;
    }

    public static RestRequest Put<T>(string resource, T body) where T : class
    {
        var request = new RestRequest(resource, Method.Put);
        request.AddJsonBody(JsonConvert.SerializeObject(body));
        return request;
    }

    public static RestRequest Delete(string resource) =>
        new RestRequest(resource, Method.Delete);
}
