using UniModule.UnityTools.ResourceSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace Modules.UniTools.UniResourceSystem
{
    public class EditorResource : ResourceItem
    {
        public string AssetPath => assetPath;

        public bool IsInstance => string.IsNullOrEmpty(AssetPath);

        public Object Target => asset;
        
        protected override void OnUpdateAsset(Object targetAsset)
        {

            
            assetPath =  AssetDatabase.GetAssetPath(targetAsset);

            if (!string.IsNullOrEmpty(assetPath)) return;

            if (!(targetAsset is GameObject targetGameObject)) return;
            
            var isPrefabAsset = PrefabUtility.IsPartOfPrefabAsset(targetGameObject);
                    
            var graphAssetPath = isPrefabAsset
                ? AssetDatabase.GetAssetPath(targetGameObject)
                : PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(targetGameObject);

            var originParent = PrefabUtility.GetCorrespondingObjectFromSource(targetGameObject);
            assetPath = graphAssetPath;
            asset = originParent;
        }

        protected override TResult LoadAsset<TResult>()
        {
            if (string.IsNullOrEmpty(AssetPath))
                return null;
            
            var asset = AssetDatabase.LoadAssetAtPath<Object>(AssetPath);
            
            return GetTargetFromSource<TResult>(asset);
        }
    }
}
