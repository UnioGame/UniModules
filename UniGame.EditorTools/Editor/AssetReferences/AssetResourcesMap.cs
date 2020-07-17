namespace UniModules.UniGame.EditorTools.Editor.AssetReferences
{
    using System;
    using Core.EditorTools.Editor.EditorResources;
    using UniGreenModules.UniGame.Core.Runtime.DataStructure;
    using Object = UnityEngine.Object;

    [Serializable]
    public class AssetResourcesMap : SerializableDictionary<Object,ResourceItem>
    {
        public AssetResourcesMap(int capacity)
            : base(capacity)
        {
            
        }
    }
}