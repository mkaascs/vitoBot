using System.Net;
using System.Text;
using Domain.VitoAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Infrastructure.Services.VitoAPI.Extensions;

internal static class HttpClientExtensions
{
    public static async Task<Response<T>> GetAsync<T>(
        this HttpClient httpClient,
        string apiEndpointPath,
        CancellationToken cancellationToken = default)
    {
        
        HttpResponseMessage responseMessage = await httpClient
            .GetAsync(apiEndpointPath, cancellationToken);
        
        return await responseMessage.ToResponse<T>(cancellationToken);
    }

    public static async Task<Response> PostAsync<T>(
        this HttpClient httpClient, 
        T message,
        string apiEndpointPath,
        CancellationToken cancellationToken = default)
    {
        
        string jsonObject = JsonConvert.SerializeObject(message, new StringEnumConverter());
        StringContent jsonRequest = new StringContent(jsonObject, Encoding.UTF8, "application/json");

        HttpResponseMessage responseMessage = await httpClient
            .PostAsync(apiEndpointPath, jsonRequest, cancellationToken);

        return new Response
        {
            StatusCode = responseMessage.StatusCode,
            IsSuccess = responseMessage.StatusCode is >= HttpStatusCode.OK and < HttpStatusCode.BadRequest
        };
    }
}