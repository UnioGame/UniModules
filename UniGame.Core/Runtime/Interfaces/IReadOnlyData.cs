namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    public interface IReadOnlyData
    {
        TData Get<TData>();
        bool Contains<TData>();
    }
}