using UniModule.UnityTools.ResourceSystem;
using UnityEditor;
using UnityEngine;

namespace Modules.UniTools.UniResourceSystem
{
    public class EditorResource : ResourceItem
    {
        public string AssetPath => assetPath;

        public bool IsInstance => string.IsNullOrEmpty(AssetPath);

        public Object Target => asset;
        
        protected override void OnUpdateAsset(Object targetAsset)
        {

            assetPath = AssetDatabase.GetAssetPath(targetAsset);

            if (string.IsNullOrEmpty(assetPath))
            {
                if (targetAsset is GameObject targetGameObject)
                {
                    var originParent = PrefabUtility.GetCorrespondingObjectFromSource(targetGameObject);
                    assetPath = AssetDatabase.GetAssetPath(originParent);
                    asset = originParent;
                }   
            }
            
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
