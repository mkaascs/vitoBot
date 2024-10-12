using System.Net;

namespace Domain.API;

public class Response<TContent> {
    public HttpStatusCode StatusCode { get; init; }
    
    public TContent? Content { get; set; }
}