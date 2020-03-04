namespace Taktika.MVVM.Runtime.Settings
{
    using System;
    using UnityEngine.AddressableAssets;

    [Serializable]
    public class UiViewsSourceReference : AssetReferenceT<UiViewsSource>
    {
        public UiViewsSourceReference(string guid) : base(guid) { }
    }
}