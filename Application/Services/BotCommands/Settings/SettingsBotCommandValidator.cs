using FluentValidation;

using Application.Abstractions.BotCommands;

namespace Application.Services.BotCommands.Settings;

public class SettingsBotCommandValidator : AbstractValidator<IBotCommandHandlingContext>
{
    public SettingsBotCommandValidator(int propertiesQuantity)
    {
        if (propertiesQuantity < 1)
            throw new ArgumentException($"{nameof(propertiesQuantity)} must be more than 0");
        
        RuleFor(context => context.Arguments)
            .NotEmpty()
            .Must(arguments =>
            {
                if (arguments.Length == 0)
                    return false;
                
                return int.TryParse(arguments[0], out int counter)
                       && counter > 0 && counter <= propertiesQuantity;
            }).WithMessage($"\u274c Порядковый номер должен быть целым числом от 1 до {propertiesQuantity} включительно");

        RuleFor(context => context.Arguments)
            .NotEmpty()
            .Must(arguments => 
            {
                if (arguments.Length < 2)
                    return false;
                
                return decimal.TryParse(arguments[1], out decimal newChance)
                       && newChance is >= 0M and <= 1M;
            }).WithMessage("\u274c Новый шанс должен быть дробным числом от 0 до 1 включительно");
    }
}