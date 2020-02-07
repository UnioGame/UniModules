namespace UniGreenModules.UniGame.SerializableContext.Runtime.AssetTypes
{
    using System;
    using Abstract;

    [Serializable]
    public abstract class TypeValueDefaultAsset<TValue, TApiValue> :
        TypeValueAsset<TValue, TApiValue>
        where TValue :class, TApiValue, new()
    {
        protected override TApiValue GetDefaultValue() {
            return defaultValue ?? new TValue();
        }
    }
}