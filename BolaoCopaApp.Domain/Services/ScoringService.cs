using BolaoCopaApp.Domain.Entities;

namespace BolaoCopaApp.Domain.Services;

public class ScoringService
{
    public int CalculateMatchScore(int predictionHome, int predictionAway, int actualHome, int actualAway)
    {
        if (predictionHome == actualHome && predictionAway == actualAway)
        {
            return 15; // Exact match
        }

        int predictionDiff = predictionHome - predictionAway;
        int actualDiff = actualHome - actualAway;

        bool predictedHomeWin = predictionDiff > 0;
        bool predictedAwayWin = predictionDiff < 0;
        bool predictedDraw = predictionDiff == 0;

        bool actualHomeWin = actualDiff > 0;
        bool actualAwayWin = actualDiff < 0;
        bool actualDraw = actualDiff == 0;

        bool correctResult = (predictedHomeWin && actualHomeWin) ||
                             (predictedAwayWin && actualAwayWin) ||
                             (predictedDraw && actualDraw);

        if (correctResult)
        {
            if (predictionDiff == actualDiff)
            {
                return 10; // Correct result and goal difference
            }
            return 5; // Correct result only
        }

        return 0; // Wrong everything
    }

    public int CalculateGroupRankScore(
        string predictedFirst, string predictedSecond, string? predictedThird, string? predictedFourth,
        string actualFirst, string actualSecond, string? actualThird, string? actualFourth)
    {
        var actualQualified = new[] { actualFirst, actualSecond, actualThird, actualFourth }
            .Where(t => t != null).ToHashSet()!;

        int score = 0;

        if (predictedFirst == actualFirst) score += 20;
        else if (actualQualified.Contains(predictedFirst)) score += 5;

        if (predictedSecond == actualSecond) score += 20;
        else if (actualQualified.Contains(predictedSecond)) score += 5;

        if (predictedThird != null && actualThird != null)
        {
            if (predictedThird == actualThird) score += 20;
            else if (actualQualified.Contains(predictedThird)) score += 5;
        }

        if (predictedFourth != null && actualFourth != null)
        {
            if (predictedFourth == actualFourth) score += 20;
            else if (actualQualified.Contains(predictedFourth)) score += 5;
        }

        return score;
    }

    public int CalculateKnockoutScore(string predictedWinner, int? predictedHome, int? predictedAway, string actualWinner, int? actualHome, int? actualAway)
    {
        if (predictedWinner != actualWinner) return 0;
        if (predictedHome.HasValue && predictedAway.HasValue &&
            actualHome.HasValue && actualAway.HasValue &&
            predictedHome == actualHome && predictedAway == actualAway) return 20;
        return 15;
    }

    public int CalculateSpecialScore(SpecialPrediction prediction, SpecialPrediction actual)
    {
        int score = 0;
        if (prediction.Champion != null && prediction.Champion == actual.Champion) score += 75;
        if (prediction.RunnerUp != null && prediction.RunnerUp == actual.RunnerUp) score += 55;
        if (prediction.MVP != null && prediction.MVP == actual.MVP) score += 50;
        if (prediction.TopScorer != null && prediction.TopScorer == actual.TopScorer) score += 45;
        if (prediction.MostAssists != null && prediction.MostAssists == actual.MostAssists) score += 45;
        if (prediction.OtherFinalist != null && prediction.OtherFinalist == actual.OtherFinalist) score += 40;
        if (prediction.GoldenBoy != null && prediction.GoldenBoy == actual.GoldenBoy) score += 40;
        if (prediction.ThirdPlace != null && prediction.ThirdPlace == actual.ThirdPlace) score += 30;

        return score;
    }
}
