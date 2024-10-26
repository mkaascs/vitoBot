using Microsoft.Extensions.Logging;

using Domain.VitoAPI;

using Application.DTO.Commands;

using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace Infrastructure.Services.TelegramAPI;

public class TelegramBotMessageSender(TelegramBotClient botClient, ILogger logger) 
{
    public async Task SendMessageAsync(
        long chatId,
        SendMessageCommand command, 
        CancellationToken cancellationToken = default) 
    {
        try
        {
            await _messageSenders[command.Type](chatId, command.Content, cancellationToken);
        }

        catch (ApiRequestException exception)
        {
            logger.LogWarning(
                "An {exceptionName} was caught. Sending messages will resume only after {time} seconds",
                nameof(ApiRequestException),
                exception.Parameters?.RetryAfter);

            await Task.Delay((exception.Parameters?.RetryAfter ?? 0) * 1000, cancellationToken);
            await _messageSenders[command.Type](chatId, command.Content, cancellationToken);
        }
    }
    
    private delegate Task SendMessageDelegate(
        long chatId,
        string content,
        CancellationToken cancellationToken);
    
    private readonly Dictionary<ContentType, SendMessageDelegate> _messageSenders = new()
    {
        { 
            ContentType.Text, async (chatId, content, cancellationToken) 
                => await botClient.SendTextMessageAsync(new ChatId(chatId), content, cancellationToken: cancellationToken) 
        },
        { 
            ContentType.Gif, async (chatId, content, cancellationToken) 
                => await botClient.SendAnimationAsync(new ChatId(chatId), new InputFileId(content), cancellationToken: cancellationToken) 
        },
        { 
            ContentType.Picture, async (chatId, content, cancellationToken)
                => await botClient.SendPhotoAsync(new ChatId(chatId), new InputFileId(content), cancellationToken: cancellationToken)
        },
        {
            ContentType.Video, async (chatId, content, cancellationToken) 
                => await botClient.SendVideoAsync(new ChatId(chatId), new InputFileId(content), cancellationToken: cancellationToken) 
        },
        { 
            ContentType.Sticker, async (chatId, content, cancellationToken) 
                => await botClient.SendStickerAsync(new ChatId(chatId), new InputFileId(content), cancellationToken: cancellationToken)
        }
    };
}