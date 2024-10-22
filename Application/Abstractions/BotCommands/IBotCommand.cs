namespace Application.Abstractions.BotCommands;

public interface IBotCommand {
    Task CallAsync(IBotCommandHandlingContext context, CancellationToken cancellationToken = default);
}