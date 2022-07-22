using System;

namespace Code.Services;

public class GameProgress
{
    //todo: change Action<int>+int to smth from UniTask or create class<T>{value;ChangedEvent}
    public event Action<int> ScoreChanged;
    public event Action<int> RestartNumberChanged;

    public int Score { get; private set; }
    public int RestartNumber { get; private set; }

    public void IncreaseScore()
    {
        Score++;
        ScoreChanged?.Invoke(Score);
    }

    public void IncreaseRestartNumber()
    {
        RestartNumber++;
        RestartNumberChanged?.Invoke(RestartNumber);
    }

    public void ResetScore()
    {
        Score = -1;
        //ScoreChanged?.Invoke(Score); //todo
    }
}
