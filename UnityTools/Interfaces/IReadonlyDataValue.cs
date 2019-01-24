namespace UniModule.UnityTools.Interfaces
{
    public interface IReadonlyDataValue<TData>
    {
        TData Value { get; }
    }
}