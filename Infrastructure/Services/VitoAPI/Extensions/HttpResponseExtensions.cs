using System.Net;
using Domain.VitoAPI;
using Newtonsoft.Json;

namespace Infrastructure.Services.VitoAPI.Extensions;

internal static class HttpResponseExtensions {
    public static async Task<Response<T>> ToResponse<T>(this HttpResponseMessage httpResponse, CancellationToken cancellationToken=default) {
        Response<T> response = new() {
            StatusCode = httpResponse.StatusCode
        };

        if (response.StatusCode != HttpStatusCode.OK)
            return response;

        response.Content = JsonConvert.DeserializeObject<T>(
            await httpResponse.Content.ReadAsStringAsync(cancellationToken));

        return response;
    }
}