using Domain.VitoAPI;

using Application.Abstractions;
using Application.DTO.Commands;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace Infrastructure.Services.TelegramAPI.Application;

public class BotMessageSender(BotClient botClient) : IMessageSender {
    private delegate Task SendMessageDelegate(long chatId, string content, CancellationToken cancellationToken);
    
    private readonly Dictionary<ContentType, SendMessageDelegate> _messageSenders = new() {
        { ContentType.Text, async (chatId, content, cancellationToken) 
            => await botClient.Bot.SendTextMessageAsync(new ChatId(chatId), content, cancellationToken: cancellationToken) },

        { ContentType.Gif, async (chatId, content, cancellationToken) 
            => await botClient.Bot.SendAnimationAsync(new ChatId(chatId), new InputFileId(content), cancellationToken: cancellationToken) },

        { ContentType.Picture, async (chatId, content, cancellationToken)
            => await botClient.Bot.SendPhotoAsync(new ChatId(chatId), new InputFileId(content), cancellationToken: cancellationToken) },

        { ContentType.Video, async (chatId, content, cancellationToken) 
            => await botClient.Bot.SendVideoAsync(new ChatId(chatId), new InputFileId(content), cancellationToken: cancellationToken) },

        { ContentType.Sticker, async (chatId, content, cancellationToken) 
            => await botClient.Bot.SendStickerAsync(new ChatId(chatId), new InputFileId(content), cancellationToken: cancellationToken) }
    };

    public async Task SendMessageAsync(SendMessageCommand command, CancellationToken cancellationToken = default)
        => await _messageSenders[command.Type](command.To, command.Content, cancellationToken);
}