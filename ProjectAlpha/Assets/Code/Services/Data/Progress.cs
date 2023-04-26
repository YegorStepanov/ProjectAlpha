namespace Code.Services.Data
{
    public class Progress : IProgress
    {
        public ISessionProgress Session { get; }
        public IPersistentProgress Persistant { get; }

        public Progress(ISessionProgress sessionProgress, IPersistentProgress persistantProgress)
        {
            Session = sessionProgress;
            Persistant = persistantProgress;
        }
    }
}
