namespace UniGreenModules.UniGame.SerializableContext.Runtime.Abstract
{
    using System;

    [Serializable]
    public abstract class TypeValueDefaultAsset<TValue, TApiValue> :
        TypeValueAsset<TValue, TApiValue>
        where TValue : TApiValue, new()
    {
        protected override TApiValue GetDefaultValue() {
            return new TValue();
        }
    }
    
    [Serializable]
    public abstract class TypeValueDefaultAsset<TValue> : 
        TypeValueDefaultAsset<TValue, TValue> 
        where TValue : new() { }
}