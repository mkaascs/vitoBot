using System.Net;

namespace Domain.VitoAPI;

/// <summary>
/// An instance of API response
/// </summary>
/// <typeparam name="TContent">Type of response content</typeparam>
public class Response<TContent> {
    public HttpStatusCode StatusCode { get; init; }
    
    public TContent? Content { get; set; }
}