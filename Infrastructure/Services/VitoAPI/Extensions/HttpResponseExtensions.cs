using System.ComponentModel;
using System.Net;
using Domain.VitoAPI;
using Newtonsoft.Json;

namespace Infrastructure.Services.VitoAPI.Extensions;

internal static class HttpResponseExtensions
{
    public static async ValueTask<Response<T>> ToResponse<T>(
        this HttpResponseMessage httpResponse,
        CancellationToken cancellationToken = default)
    {

        Response response = httpResponse.ToResponse();
        Response<T> valueResponse = new Response<T>
        {
            StatusCode = response.StatusCode,
            IsSuccess = response.IsSuccess,
            Content = default
        };

        if (!response.IsSuccess)
            return valueResponse;

        valueResponse.Content = JsonConvert.DeserializeObject<T>(
            await httpResponse.Content.ReadAsStringAsync(cancellationToken));

        return valueResponse;
    }

    private static Response ToResponse(this HttpResponseMessage httpResponse)
    {
        return new Response
        {
            StatusCode = httpResponse.StatusCode,
            IsSuccess = httpResponse.StatusCode is >= HttpStatusCode.OK and < HttpStatusCode.BadRequest
        };
    }
}