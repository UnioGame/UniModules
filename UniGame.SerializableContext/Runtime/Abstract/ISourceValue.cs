namespace UniGreenModules.UniGame.SerializableContext.Runtime.Abstract
{
    using Core.Runtime.Interfaces;
    using UniContextData.Runtime.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.Interfaces.Rx;

    public interface ISourceValue<TApiValue> : 
        IObservableValue<TApiValue>, 
        IPrototype<ISourceValue<TApiValue>>,
        IAsyncContextDataSource
    {
    }
}