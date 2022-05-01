
namespace Code;

//Key?
public readonly record struct Address(string Key)
{
    public static implicit operator Address(string key) => new(key);
}