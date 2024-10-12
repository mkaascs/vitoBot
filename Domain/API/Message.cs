namespace Domain.API;

public record Message(
    string Content, 
    ContentType Type);