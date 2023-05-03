using System;

namespace Highscore.Wpf.Service;

internal class ValidationService
{
    internal string IsHighscoreValid(Data.Model.Highscore highscore)
    {
        if(IsEmpty(highscore.PlayerName) || !IsInRange(highscore.PlayerName, 3, 30))
            return "Player name empty or out of range - min: 3 | max: 30";

        if (IsEmpty(highscore.GameName))
            return "Game name empty";

        if (!IsInteger(highscore.ScoreValue))
            return "Score is not a valid integer";

        if (IsEmpty(highscore.Comment))
            return "Comment is empty";

        if (highscore.Date == DateTime.MinValue || highscore.Date == DateTime.MaxValue)
            return "Date was min or max value";

        return string.Empty;
    }

    bool IsEmpty(string value) => string.IsNullOrEmpty(value);

    bool IsInRange(string value, int min, int max) => !(value.Length < min || value.Length > max);

    bool IsInteger(string value) => int.TryParse(value, out var result);
}
