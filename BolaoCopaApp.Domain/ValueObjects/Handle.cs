using System.Text.RegularExpressions;

namespace BolaoCopaApp.Domain.ValueObjects;

public record Handle
{
    public string Value { get; init; }

    public Handle(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Handle cannot be empty.");

        if (value.Length < 3 || value.Length > 20)
            throw new ArgumentException("Handle must be between 3 and 20 characters.");

        if (!value.StartsWith("@"))
            throw new ArgumentException("Handle must start with @.");

        Value = value.ToLowerInvariant();
    }

    public static implicit operator string(Handle handle) => handle.Value;
    public static implicit operator Handle(string value) => new Handle(value);
}
