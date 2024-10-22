using System.Net;
using System.Text;
using Domain.VitoAPI;
using Newtonsoft.Json;

namespace Infrastructure.Services.VitoAPI.Extensions;

internal static class HttpClientExtensions {
    public static async Task<Response<T>> GetAsync<T>(this HttpClient httpClient, string apiEndpointPath,
        CancellationToken cancellationToken = default) {
        
        HttpResponseMessage responseMessage = await httpClient.GetAsync(apiEndpointPath, cancellationToken);
        return await responseMessage.ToResponse<T>(cancellationToken);
    }

    public static async Task<Response<bool>> PostAsync<T>(this HttpClient httpClient, string apiEndpointPath,
        T message, CancellationToken cancellationToken = default) {
        
        string jsonObject = JsonConvert.SerializeObject(message);
        StringContent jsonRequest = new StringContent(jsonObject, Encoding.UTF8, "application/json");

        HttpResponseMessage responseMessage =
            await httpClient.PostAsync(apiEndpointPath, jsonRequest, cancellationToken);

        return new Response<bool> {
            StatusCode = responseMessage.StatusCode,
            Content = responseMessage.StatusCode is >= HttpStatusCode.OK and < HttpStatusCode.BadRequest
        };
    }
}