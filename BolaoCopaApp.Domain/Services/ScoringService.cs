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

    public int CalculateGroupRankScore(string predictedFirst, string predictedSecond, string actualFirst, string actualSecond)
    {
        int score = 0;
        if (predictedFirst == actualFirst) score += 10;
        else if (predictedFirst == actualSecond) score += 5;

        if (predictedSecond == actualSecond) score += 10;
        else if (predictedSecond == actualFirst) score += 5;

        return score;
    }

    public int CalculateKnockoutScore(string predictedWinner, string actualWinner)
    {
        return predictedWinner == actualWinner ? 10 : 0;
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
