namespace Assets.Tools.UnityTools.Interfaces
{
    public interface IReadonlyDataValue<TData>
    {
        TData Value { get; }
    }
}