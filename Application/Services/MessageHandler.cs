using Microsoft.Extensions.Logging;

using Application.Abstractions;
using Application.Abstractions.BotCommands;
using Application.DTO.Commands;
using Application.Services.BotLogic;

namespace Application.Services;

public class MessageHandler(MessageBotLogic botLogic, ILogger<MessageHandler> logger) : IMessageHandler {
    public async Task OnGetMessageAsync(IMessageHandlingContext context, CancellationToken cancellationToken = default) {
        if (context is IBotCommandHandlingContext)
            return;
        
        IList<SendMessageCommand> answers = (await botLogic.GetAnswerAsync(context.Message, cancellationToken)).ToList();

        IList<Task> replyingTasks = new List<Task>(answers.Count);
        foreach (SendMessageCommand answer in answers)
            replyingTasks.Add(context.AnswerAsync(answer, cancellationToken));

        await Task.WhenAll(replyingTasks);
        
        logger.LogInformation("{count} messages were sent to {chat}", answers.Count, context.Message.From?.FirstName);
    }
}