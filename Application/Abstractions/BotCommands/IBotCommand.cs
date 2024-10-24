namespace Application.Abstractions.BotCommands;

/// <summary>
/// An interface which represents the bot command
/// </summary>
public interface IBotCommand
{
    /// <summary>
    /// Executes when this bot command was called
    /// </summary>
    /// <param name="context">Context of bot command handling</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Returns completed task</returns>
    Task CallAsync(IBotCommandHandlingContext context, CancellationToken cancellationToken = default);
}