namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    public interface IReadonlyDataValue<out TData>
    {
        TData Value { get; }
    }
}