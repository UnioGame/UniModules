namespace UniGreenModules.UniGame.SerializableContext.Runtime.AssetTypes
{
    using System;

    [Serializable]
    public abstract class TypeValueDefaultAsset<TValue> : 
        TypeValueDefaultAsset<TValue, TValue> 
        where TValue :class, new(){ }
}