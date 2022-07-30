namespace Code.Services.Data;

public class Progress : IProgress
{
    public ISessionProgress Session { get; }
    public IPersistentProgress Persistant { get; }

    //mb hide Session/Persistant, only methods?
    public Progress(ISessionProgress sessionProgress, IPersistentProgress persistantProgress)
    {
        Session = sessionProgress;
        Persistant = persistantProgress;
    }
}
