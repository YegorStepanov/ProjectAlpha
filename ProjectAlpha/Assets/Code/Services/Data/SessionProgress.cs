using System;

namespace Code.Services;

public class SessionProgress : ISessionProgress
{
    public event Action ScoreChanged;
    public event Action RestartNumberChanged;

    private int _score;
    private int _restartNumber;

    public int Score
    {
        get => _score;
        private set
        {
            _score = value;
            ScoreChanged?.Invoke();
        }
    }
    public int RestartNumber
    {
        get => _restartNumber;
        private set
        {
            _restartNumber = value;
            RestartNumberChanged?.Invoke();
        }
    }

    public void IncreaseScore() =>
        Score++;

    public void IncreaseRestartNumber() =>
        RestartNumber++;

    public void ResetScore() =>
        Score = 0;
}
