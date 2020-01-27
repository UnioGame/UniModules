namespace UniGreenModules.UniGame.SerializableContext.Runtime.Abstract
{
    using System;

    [Serializable]
    public abstract class TypeDataDefaultAsset<TValue, TApiValue> :
        TypeDataAsset<TValue, TApiValue>
        where TValue : TApiValue, new()
    {
        protected override TApiValue GetDefaultValue() => new TValue();
    }
    
    [Serializable]
    public abstract class TypeDataDefaultAsset<TValue> : 
        TypeDataDefaultAsset<TValue, TValue> 
        where TValue : new() { }
}