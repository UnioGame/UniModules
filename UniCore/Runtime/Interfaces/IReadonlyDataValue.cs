namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    public interface IReadonlyDataValue<TData>
    {
        TData Value { get; }
    }
}