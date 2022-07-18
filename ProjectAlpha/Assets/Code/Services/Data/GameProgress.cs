using System;

namespace Code.Services;

public class GameProgress
{
    public event Action<int> ScoreChanged;

    public int Score { get; private set; }

    public GameProgress() =>
        Initialize();

    private void Initialize()
    {
        Score = -1;
    }

    public void IncreaseScore()
    {
        Score++;
        ScoreChanged?.Invoke(Score);
    }
}
