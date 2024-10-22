using Application.Abstractions;
using Application.DTO;
using Application.DTO.Commands;

using Infrastructure.Services.TelegramAPI.Extensions;

using Telegram.Bot.Types;

namespace Infrastructure.Services.TelegramAPI.Application;

public class TelegramMessageHandlingContext : IMessageHandlingContext {
    private readonly TelegramBotMessageSender _messageSender;

    public TelegramMessageHandlingContext(Message message, TelegramBotMessageSender messageSender) {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(messageSender);

        (Message, _messageSender) = (message.ToDto(), messageSender);
    }
    
    public MessageDto Message { get; set; }
    public async Task AnswerAsync(SendMessageCommand command, CancellationToken cancellationToken = default) 
        => await _messageSender.SendMessageAsync((long)Message.Chat.Id, command, cancellationToken);
}