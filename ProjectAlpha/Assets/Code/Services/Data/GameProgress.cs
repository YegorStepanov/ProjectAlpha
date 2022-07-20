using System;

namespace Code.Services;

public class GameProgress
{
    public event Action<int> ScoreChanged;

    public int Score { get; private set; }

    public void IncreaseScore()
    {
        Score++;
        ScoreChanged?.Invoke(Score);
    }

    public void ResetScore()
    {
        Score = -1;
        //ScoreChanged?.Invoke(Score); //todo
    }
}
