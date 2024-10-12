using ContentType = Domain.VitoAPI.ContentType;

using Application.DTO;

using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Infrastructure.Services.TelegramAPI.Extensions;

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
        
        switch (message.Type) {
            case MessageType.Text:
                content = message.Text;
                type = ContentType.Text;
                break;
            
            case MessageType.Sticker:
                content = message.Sticker?.FileId;
                type = ContentType.Sticker;
                break;
            
            case MessageType.Animation:
                content = message.Animation?.FileId;
                type = ContentType.Gif;
                break;
            
            case MessageType.Photo:
                content = message.Photo?.FirstOrDefault()?.FileId;
                type = ContentType.Picture;
                break;
            
            case MessageType.Video:
                content = message.Video?.FileId;
                type = ContentType.Video;
                break;
        }
    }
}