using System.Net;

namespace Domain.VitoAPI;

/// <summary>
/// An instance of API response
/// </summary>
public class Response
{
    public bool IsSuccess { get; init; }

    public HttpStatusCode StatusCode { get; init; }
}

/// <summary>
/// An instance of API response
/// </summary>
/// <typeparam name="TContent">Type of response content</typeparam>
public class Response<TContent> : Response
{
    public TContent? Content { get; set; }
}