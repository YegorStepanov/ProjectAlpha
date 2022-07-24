using System;

namespace Code.Services;

public class SessionProgress : ISessionProgress
{
    public event Action ScoreChanged;
    public event Action RestartNumberChanged;

    public int Score { get; private set; }
    public int RestartNumber { get; private set; }

    public void IncreaseScore()
    {
        Score++;
        ScoreChanged?.Invoke();
    }

    public void IncreaseRestartNumber()
    {
        RestartNumber++;
        RestartNumberChanged?.Invoke();
    }

    public void ResetScore()
    {
        Score = -1;
        ScoreChanged?.Invoke(); //todo
    }
}
