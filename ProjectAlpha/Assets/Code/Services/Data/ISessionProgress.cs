namespace Code.Services.Data
{
    public interface ISessionProgress
    {
        ObservedValue<int> Score { get; }
        ObservedValue<int> RestartNumber { get; }

        void IncreaseScore();
        void IncreaseRestartNumber();
        void ResetScore();
    }
}
