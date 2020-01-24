namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    public interface ICloneable<out TData>
    {
        TData Clone();
    }
}
