namespace UniModules.UniGame.SerializableContext.Runtime.Abstract
{
    using UniGreenModules.UniContextData.Runtime.Interfaces;
    using UniGreenModules.UniCore.Runtime.Interfaces.Rx;
    using UniGreenModules.UniGame.Core.Runtime.Interfaces;

    public interface ISourceValue<TApiValue> : 
        IObservableValue<TApiValue>, 
        IPrototype<ISourceValue<TApiValue>>,
        IAsyncContextDataSource
    {
    }
}