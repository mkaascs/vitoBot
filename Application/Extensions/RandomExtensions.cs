namespace Application.Extensions;

internal static class RandomExtensions {
    public static bool WithChance(this Random randomizer, double chance) {
        if (chance is < 0.0 or > 1.0)
            throw new ArgumentOutOfRangeException($"{chance} must be between 0 and 1");

        return chance > randomizer.NextDouble();
    }
}