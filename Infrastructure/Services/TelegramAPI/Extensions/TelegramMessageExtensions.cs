using Application.DTO;

using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using ContentType = Domain.VitoAPI.ContentType;

namespace Infrastructure.Services.TelegramAPI.Extensions;

internal static class TelegramMessageExtensions
{
    public static MessageDto ToDto(this Message message)
    {
        message.GetContent(out string? messageContent, out ContentType messageType);
        return new MessageDto(
            message.MessageId,
            message.From.ToDto(),
            message.Chat.ToDto(),
            message.Date,
            messageType,
            messageContent);
    }

    private static ChatDto ToDto(this Chat chat)
    {
        return new ChatDto((ulong)chat.Id, chat.Title);
    }

    private static UserDto? ToDto(this User? user)
    {
        return user is null ? null : new UserDto((ulong)user.Id, user.FirstName);
    }

    private static void GetContent(this Message message, out string? content, out ContentType type) 
    {
        content = null;
        type = ContentType.Text;
        
        switch (message) {
            case { Type: MessageType.Text }:
                content = message.Text;
                type = ContentType.Text;
                break;
            
            case { Type: MessageType.Sticker }:
                content = message.Sticker?.FileId;
                type = ContentType.Sticker;
                break;
            
            case { Type: MessageType.Animation }:
                content = message.Animation?.FileId;
                type = ContentType.Gif;
                break;
            
            case { Type: MessageType.Photo }:
                content = message.Photo?.FirstOrDefault()?.FileId;
                type = ContentType.Picture;
                break;
            
            case { Type: MessageType.Video }:
                content = message.Video?.FileId;
                type = ContentType.Video;
                break;
        }
    }
}