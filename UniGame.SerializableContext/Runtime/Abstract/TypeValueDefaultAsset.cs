namespace UniGreenModules.UniGame.SerializableContext.Runtime.AssetTypes
{
    using System;
    using Abstract;

    [Serializable]
    public abstract class TypeValueDefaultAsset<TValue, TApiValue> :
        TypeValueAssetSource<TValue, TApiValue>
        where TValue :class, TApiValue, new()
    {
        protected override TValue GetDefaultValue() {
            return defaultValue ?? new TValue();
        }
    }
}