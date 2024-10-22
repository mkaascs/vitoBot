using Domain.VitoAPI;

namespace Application.DTO.Commands;

public record SendMessageCommand(
    string Content,
    ContentType Type);