namespace Application.Extensions;

internal static class RandomExtensions {
    public static bool WithChance(this Random randomizer, decimal chance) {
        if (chance is < 0m or > 1m)
            throw new ArgumentOutOfRangeException($"{chance} must be between 0 and 1");

        return chance > (decimal)randomizer.NextDouble();
    }
}