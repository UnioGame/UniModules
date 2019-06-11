namespace UniGreenModules.UniCore.Runtime.Interfaces.Rx
{
    public interface IRecycleReactiveProperty<TValue> : 
        IReadonlyRecycleReactiveProperty<TValue>
    {
        new TValue Value { get; set; }
    }
}
