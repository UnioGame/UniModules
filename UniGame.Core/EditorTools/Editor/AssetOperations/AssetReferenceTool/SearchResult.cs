namespace UniModules.UniGame.EditorTools.Editor.AssetReferences
{
    using System;
    using System.Collections.Generic;
    using Core.EditorTools.Editor.EditorResources;
    using Object = UnityEngine.Object;

    [Serializable]
    public class SearchResult
    {
        public AssetReferencesMap referenceMap = new AssetReferencesMap(2);
        
        public AssetResourcesMap assetsInfos  = new AssetResourcesMap(2);
        
        public SearchResult AddKey(Object asset)
        {
            if (referenceMap.ContainsKey(asset)) {
                return this;
            }
            referenceMap[asset] = new List<ResourceItem>();
            assetsInfos[asset]  = new EditorResource().Update(asset);
            return this;
        }
    }
}