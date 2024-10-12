using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Domain.API;

public record Message(
    string Content, 
    ContentType Type);