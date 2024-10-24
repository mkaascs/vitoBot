namespace Application.Abstractions;

/// <summary>
/// An interface which represents general information about the user
/// </summary>
public interface IUserContext
{
    /// <summary>
    /// A user property that indicates whether the user is allowed to change user settings
    /// </summary>
    public bool AllowedToChangeUserSettings { get; protected set; }
}