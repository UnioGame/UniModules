namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using UniRx;

    public interface IReactiveValueStatus
    {
        IReadOnlyReactiveProperty<bool> HasValueSource { get; }
        
    }
    
    public interface IValueContainerStatus
    {
        bool HasValue { get; }
        
    }
}