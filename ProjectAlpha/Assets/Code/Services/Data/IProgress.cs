namespace Code.Services;

public interface IProgress
{
    public ISessionProgress Session { get; }
    public IPersistentProgress Persistant { get; }
}
