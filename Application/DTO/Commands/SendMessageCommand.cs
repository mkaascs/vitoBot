using Domain.VitoAPI;

namespace Application.DTO.Commands;

public record SendMessageCommand(
    ulong To,
    string Content,
    ContentType Type);