using Domain.Abstractions;
using Domain.Entities;
using Domain.VitoAPI;

using Application.DTO;
using Application.Extensions;

namespace Application.Services.BotLogic;

public class MessageSavingLogic(
    IChatApiService chatApiService,
    IMessageApiService messageApiService)
{
    private readonly Random _randomizer = new();
    
    public async Task<bool> TryRememberMessageAsync(
        MessageDto receivedMessage,
        UserSettings settings,
        CancellationToken cancellationToken = default)
    {
        decimal chanceToRememberMessage = receivedMessage.Type == ContentType.Text
            ? settings.ChanceToSaveTextMessage
            : settings.ChanceToSaveMessage;

        return _randomizer.WithChance(chanceToRememberMessage)
               && await RememberMessageAsync(receivedMessage, cancellationToken);
    }

    private async ValueTask<bool> RememberMessageAsync(
        MessageDto receivedMessage, 
        CancellationToken cancellationToken = default) 
    {
        if (string.IsNullOrWhiteSpace(receivedMessage.Content))
            return false;

        await RegisterNewChatIfRequiredAsync(receivedMessage, cancellationToken);
        return await messageApiService.AddNewMessageAsync(
            receivedMessage.Chat.Id,
            new Message(
                receivedMessage.Content,
                receivedMessage.Type),
            cancellationToken);
    }

    private async ValueTask RegisterNewChatIfRequiredAsync(
        MessageDto receivedMessage,
        CancellationToken cancellationToken = default)
    {
        Chat? foundChat = await chatApiService
            .GetChatByIdAsync(receivedMessage.Chat.Id, cancellationToken);
        
        if (foundChat != null)
            return;

        string? newChatName = receivedMessage.Chat.Title ?? receivedMessage.From?.FirstName;

        await chatApiService.RegisterNewChatAsync(
            new Chat(
                receivedMessage.Chat.Id,
                newChatName),
            cancellationToken);
    }
}