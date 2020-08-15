namespace UniModules.UniGame.Core.EditorTools.Editor.AssetOperations.AssetReferenceTool
{
    using System;
    using System.Collections.Generic;
    using EditorResources;
    using UniGreenModules.UniGame.Core.Runtime.DataStructure;
    using Object = UnityEngine.Object;

    [Serializable]
    public class AssetReferencesMap : SerializableDictionary<Object, List<ResourceHandle>>
    {
        public AssetReferencesMap(int capacity)
            : base(capacity)
        {
            
        }
    }
}