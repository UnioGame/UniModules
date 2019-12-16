namespace UniGreenModules.UniResourceSystem.Editor
{
    using Runtime;
    using UnityEditor;
    using UnityEngine;

    public class EditorResource : ResourceItem
    {
        public string AssetPath => assetPath;

        public bool IsInstance => string.IsNullOrEmpty(AssetPath);

        public Object Target => asset;
        
        protected override void OnUpdateAsset(Object targetAsset)
        {
            var resultAsset = targetAsset;
            var resultPath = string.Empty;

            if (targetAsset is Component componentAsset)
                resultAsset = componentAsset.gameObject;
            
            if (resultAsset is GameObject targetGameObject) {
                             
                resultPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(targetGameObject);
                
                var originParent = PrefabUtility.GetCorrespondingObjectFromSource(targetGameObject);
                
                resultAsset = 
                    originParent != null ? originParent : 
                    string.IsNullOrEmpty(resultPath) ? null :        
                    AssetDatabase.LoadAssetAtPath<GameObject>(resultPath);
                    
            }
            else {
                resultPath = AssetDatabase.GetAssetPath(resultAsset);
            }

            assetPath = resultPath;
            asset = resultAsset;
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
