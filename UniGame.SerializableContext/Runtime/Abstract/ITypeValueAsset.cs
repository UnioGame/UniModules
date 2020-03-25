namespace UniGreenModules.UniGame.SerializableContext.Runtime.Abstract
{
    using UniCore.Runtime.Interfaces;

    public interface ITypeValueAsset<TValue,TApi> : 
        IDataValue<TValue, TApi>,
        ILifeTimeContext
        where TValue : TApi
    {
    }
}