namespace UniGreenModules.UniGame.SerializableContext.Runtime.Abstract
{
    using UniCore.Runtime.Interfaces;

    public interface ITypeValueAsset<TValue> : 
        IDataValue<TValue>,
        ILifeTimeContext
    {
    }
}