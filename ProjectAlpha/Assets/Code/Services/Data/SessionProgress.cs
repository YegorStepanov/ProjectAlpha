namespace Code.Services.Data
{
    public class SessionProgress : ISessionProgress
    {
        private readonly ObservedValueWriter<int> _score = new(0);
        private readonly ObservedValueWriter<int> _restartNumber = new(0);

        public ObservedValue<int> Score => _score;
        public ObservedValue<int> RestartNumber => _restartNumber;

        public void IncreaseScore() => _score.Value++;
        public void IncreaseRestartNumber() => _restartNumber.Value++;
        public void ResetScore() => _score.Value = 0;
    }
}