using Application.DTO;

using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using ContentType = Domain.VitoAPI.ContentType;

namespace Infrastructure.Services.TelegramAPI.Application.Extensions;

internal static class TelegramMessageExtensions {
    public static MessageDto ToDto(this Message message) {
        message.GetContent(out ContentType messageType, out string? messageContent);
        return new MessageDto(
            message.MessageId,
            message.From.ToDto(),
            message.Chat.ToDto(),
            message.Date,
            messageType,
            messageContent);
    }
    
    private static ChatDto ToDto(this Chat chat)
        => new(chat.Id, chat.Title);

    private static UserDto? ToDto(this User? user)
        => user is null ? null : new UserDto(user.Id, user.FirstName);

    private static void GetContent(this Message message, out ContentType type, out string? content) {
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