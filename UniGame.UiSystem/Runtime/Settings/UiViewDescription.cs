namespace Taktika.MVVM.Runtime.Settings
{
    using System;
    using System.Collections.Generic;
    using Abstracts;
    using GameRuntime.Types;
    using GameRuntime.Types.Attributes;
    using Sirenix.OdinInspector;
    using UniGreenModules.UniGame.AddressableTools.Runtime.Attributes;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.Util;

    [Serializable]
    public class UiViewDescription
    {
        [GUIColor(g:1.0f, r: 1.0f, b:0.5f)]
        [Space(2)]
        public string Tag = string.Empty;
        
        [Space(2)]
        [DrawWithUnity]
        [STypeFilter(typeof(IView), nameof(SType.fullTypeName))]
        public SType Type;

        [Space(2)]
        [DrawWithUnity]
        [ShowAssetReference]
        public AssetReferenceGameObject View;

    }
}