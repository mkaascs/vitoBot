using Domain.VitoAPI;

namespace Application.DTO;

public record MessageDto(
    int Id,
    UserDto? From,
    ChatDto Chat,
    DateTime Date,
    ContentType Type,
    string? Content);