using Code.AddressableAssets;
using Code.Scopes;
using Code.Services;

namespace Code;

public static class DataAddress
{
    public static readonly Address<WidthGeneratorData> WidthGenerator = new("Width Generator");
    public static readonly Address<PositionGeneratorData> PositionGenerator = new("Position Generator");
    
    public static readonly Address<RootScope> RootScope = new("Root Scope");
}