namespace Modules.UnityToolsModule.Tools.UnityTools.Interfaces
{
    public interface ICloneable<out TData>
    {
        TData Clone();
    }
}
