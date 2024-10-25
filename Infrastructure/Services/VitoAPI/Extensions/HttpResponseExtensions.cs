using System.Net;
using Infrastructure.Services.VitoAPI.Responses;
using Newtonsoft.Json;

namespace Infrastructure.Services.VitoAPI.Extensions;

internal static class HttpResponseExtensions
{
    public static async ValueTask<Response> ToResponse(
        this HttpResponseMessage httpResponse,
        CancellationToken cancellationToken = default)
    {
        Response response = httpResponse.InitializeResponse();
        
        response.Problem = response.IsSuccess
            ? null
            : JsonConvert.DeserializeObject<ProblemDetails>(
                await httpResponse.Content.ReadAsStringAsync(cancellationToken));

        return response;
    }
    
    public static async ValueTask<Response<TEntity>> ToResponse<TEntity>(
        this HttpResponseMessage httpResponse,
        CancellationToken cancellationToken = default)
    {
        Response response = await httpResponse.ToResponse(cancellationToken);

        return new Response<TEntity>
        {
            StatusCode = response.StatusCode,
            IsSuccess = response.IsSuccess,
            Problem = response.Problem,
            Content = response.IsSuccess
                ? JsonConvert.DeserializeObject<TEntity>(
                    await httpResponse.Content.ReadAsStringAsync(cancellationToken))
                : default
        };
    }
    
    private static Response InitializeResponse(
        this HttpResponseMessage httpResponse)
    {
        return new Response
        {
            StatusCode = httpResponse.StatusCode,
            IsSuccess = httpResponse.StatusCode is >= HttpStatusCode.OK and < HttpStatusCode.BadRequest,
        };
    }
}