namespace UniModule.UnityTools.Interfaces
{
    public interface ICloneable<out TData>
    {
        TData Clone();
    }
}
