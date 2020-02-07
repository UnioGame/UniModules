namespace UniGreenModules.UniGame.SerializableContext.Runtime.Abstract
{
    using UniCore.Runtime.Interfaces;

    public interface ISourceValue<TApiValue> : 
        IDataValue<TApiValue>, 
        IAsyncSource
    {
    }
}