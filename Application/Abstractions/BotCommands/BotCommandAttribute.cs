namespace Application.Abstractions.BotCommands;

public class BotCommandAttribute : Attribute {
    private string _botCommandName = "/command";
    
    public BotCommandAttribute() { }

    public BotCommandAttribute(string commandName) {
        ArgumentException.ThrowIfNullOrWhiteSpace(commandName);
        _botCommandName = commandName;
    }

    public string CommandName {
        get => _botCommandName;
        set {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            _botCommandName = value;
        }
    }
}