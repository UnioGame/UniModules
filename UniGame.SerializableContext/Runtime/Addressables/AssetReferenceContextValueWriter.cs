namespace UniModules.UniGame.SerializableContext.Runtime.Addressables
{
    using System;
    using UniGreenModules.UniCore.Runtime.Interfaces;

    [Serializable]
    public class AssetReferenceContextValueWriter : AssetReferenceApiT<IValueWriter<IContext>>
    {
        public AssetReferenceContextValueWriter(string guid) : base(guid)
        {
        }
    }
}