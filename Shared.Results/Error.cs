namespace Shared.Results;

public sealed record Error
{
    public string Type { get; } = null!;

    public string Description { get; } = null!;

    public Error(string type, string description)
    {
        Type = type;
        Description = description;
    }

    public static readonly Error None = new Error(string.Empty, string.Empty);
}