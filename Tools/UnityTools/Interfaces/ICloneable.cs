namespace Assets.Tools.UnityTools.Interfaces
{
    public interface ICloneable<out TData>
    {
        TData Clone();
    }
}
