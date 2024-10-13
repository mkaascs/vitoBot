using Domain.VitoAPI;

namespace Application.DTO.Commands;

public record SendMessageCommand(
    long To,
    string Content,
    ContentType Type);