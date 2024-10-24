namespace Application.Abstractions.BotCommands;

/// <summary>
/// An interface which represents the context of the handled bot command message
/// </summary>
public interface IBotCommandHandlingContext : IMessageHandlingContext 
{
    /// <summary>
    /// Name of the command is called
    /// </summary>
    public string CommandName { get; protected set; }
    
    /// <summary>
    /// Array of arguments
    /// </summary>
    public string[] Arguments { get; protected set; }
}