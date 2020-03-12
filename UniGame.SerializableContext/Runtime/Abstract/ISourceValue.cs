namespace UniGreenModules.UniGame.SerializableContext.Runtime.Abstract
{
    using Core.Runtime.Interfaces;
    using UniContextData.Runtime.Interfaces;
    using UniCore.Runtime.Interfaces;

    public interface ISourceValue<TApiValue> : 
        IDataValue<TApiValue>, 
        IPrototype<ISourceValue<TApiValue>>,
        IAsyncContextDataSource
    {
    }
}