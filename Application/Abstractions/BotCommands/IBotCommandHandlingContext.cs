namespace Application.Abstractions.BotCommands;

public interface IBotCommandHandlingContext : IMessageHandlingContext {
    public string CommandName { get; protected set; }
    
    public string[] Arguments { get; protected set; }
}