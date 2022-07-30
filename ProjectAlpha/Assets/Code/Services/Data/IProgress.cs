namespace Code.Services.Data;

public interface IProgress
{
    public ISessionProgress Session { get; }
    public IPersistentProgress Persistant { get; }
}
