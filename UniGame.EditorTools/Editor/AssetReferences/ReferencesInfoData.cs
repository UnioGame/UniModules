namespace UniModules.UniGame.EditorTools.Editor.AssetReferences
{
    using System;
    using System.Collections.Generic;
    using Core.EditorTools.Editor.EditorResources;
    using UnityEngine;
    using Object = UnityEngine.Object;

    [Serializable]
    public class ReferencesInfoData
    {
        [SerializeField]
        [Sirenix.OdinInspector.HideLabel]
        [Sirenix.OdinInspector.InlineProperty]
        public ResourceHandle source;
        
        [Space(4)]
        [SerializeField]
        public List<Object> references = new List<Object>();
    }
}