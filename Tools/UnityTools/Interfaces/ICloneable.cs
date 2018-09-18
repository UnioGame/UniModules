namespace Assets.Scripts.Interfaces
{
    public interface ICloneable<out TData>
    {
        TData Clone();
    }
}
