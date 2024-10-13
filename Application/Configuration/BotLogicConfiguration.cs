using System.ComponentModel.DataAnnotations;

namespace Application.Configuration;

public class BotLogicConfiguration {
    private readonly double _defaultChanceToSendMessage;
    private readonly double _chanceToSaveTextMessage;
    private readonly double _chanceToSaveMessage;

    /// <summary>
    /// A value between 0 and 1.0 defining default chance to send message
    /// </summary>
    [Range(0.0, 1.0)]
    public double DefaultChanceToSendMessage {
        get => _defaultChanceToSendMessage;
        init => _defaultChanceToSendMessage = value > 1.0
            ? 1.0
            : value < 0.0
                ? 0.0
                : value;
    }

    /// <summary>
    /// A value between 0 and 1.0 defining chance to save text message
    /// </summary>
    [Range(0.0, 1.0)]
    public double ChanceToSaveTextMessage {
        get => _chanceToSaveTextMessage;
        init => _chanceToSaveTextMessage = value > 1.0
            ? 1.0
            : value < 0.0
                ? 0.0
                : value;
    }

    /// <summary>
    /// A value between 0 and 1.0 defining chance to save non-text message
    /// </summary>
    [Range(0.0, 1.0)]
    public double ChanceToSaveMessage {
        get => _chanceToSaveMessage;
        init => _chanceToSaveMessage = value > 1.0
            ? 1.0
            : value < 0.0
                ? 0.0
                : value;
    }
}