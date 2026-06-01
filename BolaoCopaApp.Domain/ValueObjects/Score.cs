namespace BolaoCopaApp.Domain.ValueObjects;

public record Score
{
    public int Value { get; init; }

    public Score(int value)
    {
        if (value < 0 || value > 20)
            throw new ArgumentException("Score must be between 0 and 20.");
        
        Value = value;
    }

    public static implicit operator int(Score score) => score.Value;
    public static implicit operator Score(int value) => new Score(value);
}
