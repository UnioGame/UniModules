namespace UniModules.UniGame.EditorTools.Editor.AssetReferences
{
    using System;
    using System.Collections.Generic;
    using Core.EditorTools.Editor.EditorResources;
    using UniGreenModules.UniGame.Core.Runtime.DataStructure;
    using Object = UnityEngine.Object;

    [Serializable]
    public class AssetReferencesMap : SerializableDictionary<Object, List<ResourceItem>>
    {
        public AssetReferencesMap(int capacity)
            : base(capacity)
        {
            
        }
    }
}