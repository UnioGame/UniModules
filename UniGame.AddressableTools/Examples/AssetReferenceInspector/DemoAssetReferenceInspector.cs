using UnityEngine;

namespace UniGreenModules.UniGame.AddressableTools.Editor.AddressablesAssetDrawer.Examples
{
        using System.Collections.Generic;
        using Runtime.Attributes;
    using UnityEngine.AddressableAssets;

    public class DemoAssetReferenceInspector : MonoBehaviour
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.DrawWithUnity]
#endif
        [ShowAssetReference]
        public AssetReference Demo1;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.DrawWithUnity]
#endif
        [ShowAssetReference]
        public List<AssetReference> DemoList = new List<AssetReference>();
        
    }
}
