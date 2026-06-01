using System.Text.RegularExpressions;

namespace BolaoCopaApp.Domain.ValueObjects;

public record Handle
{
    public string Value { get; init; }

    public Handle(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Handle cannot be empty.");

        // Normalize: ensures it starts with @ and is lowercase
        var normalized = value.Trim().ToLowerInvariant();
        if (!normalized.StartsWith("@"))
        {
            normalized = "@" + normalized;
        }

        if (normalized.Length < 3 || normalized.Length > 20)
            throw new ArgumentException("Handle (including @) must be between 3 and 20 characters.");

        Value = normalized;
    }

    public static implicit operator string(Handle handle) => handle.Value;
    public static implicit operator Handle(string value) => new Handle(value);
}
