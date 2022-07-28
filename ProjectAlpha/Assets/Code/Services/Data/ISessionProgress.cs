﻿using System;

namespace Code.Services;

public interface ISessionProgress
{
    event Action ScoreChanged;
    event Action RestartNumberChanged;
    
    int Score { get; }
    int RestartNumber { get; }
    
    void IncreaseScore();
    void IncreaseRestartNumber();
    void ResetScore();
}