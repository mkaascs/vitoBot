using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Infrastructure.Services.VitoAPI.Extensions;

internal static class HttpClientExtensions
{
    public static async ValueTask<HttpResponseMessage> GetAsync(
        this HttpClient httpClient,
        string apiEndpointPath,
        CancellationToken cancellationToken = default)
    {
        return await httpClient
            .GetAsync(apiEndpointPath, cancellationToken);
    }

    public static async ValueTask<HttpResponseMessage> PostAsync<T>(
        this HttpClient httpClient, 
        T message,
        string apiEndpointPath,
        CancellationToken cancellationToken = default)
    {
        string jsonObject = JsonConvert.SerializeObject(message, new StringEnumConverter());
        StringContent jsonRequest = new StringContent(jsonObject, Encoding.UTF8, "application/json");

        return await httpClient
            .PostAsync(apiEndpointPath, jsonRequest, cancellationToken);
    }
}