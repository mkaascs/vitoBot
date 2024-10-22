using Application.Abstractions;
using Application.DTO;
using Application.DTO.Commands;

using Infrastructure.Services.TelegramAPI.Extensions;

using Telegram.Bot.Types;

namespace Infrastructure.Services.TelegramAPI.Application;

public class TelegramMessageHandlingContext(TelegramUserContext userContext, Message message, TelegramBotMessageSender messageSender) : IMessageHandlingContext {
    
    public IUserContext User { get; set; } = userContext;

    public MessageDto Message { get; set; } = message.ToDto();

    public async Task AnswerAsync(SendMessageCommand command, CancellationToken cancellationToken = default) 
        => await messageSender.SendMessageAsync((long)Message.Chat.Id, command, cancellationToken);
}