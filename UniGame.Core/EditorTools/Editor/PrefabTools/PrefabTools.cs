namespace UniGreenModules.UniCore.EditorTools.Editor.PrefabTools
{
    using System;
    using Runtime.ProfilerTools;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;

    public static class PrefabTools 
    {

        public static EditorPrefabDefinition GetPrefabDefinition(this Component component)
        {
            return GetPrefabDefinition(component.gameObject);
        }
        
        public static EditorPrefabDefinition GetPrefabDefinition(this GameObject target)
        {
            var definition = new EditorPrefabDefinition();
            if (!target) return definition;
            
            definition.Asset = target;

            var instanceStatus  = PrefabUtility.GetPrefabInstanceStatus(target);
            var prefabAssetType = PrefabUtility.GetPrefabAssetType(target);
            var isVariant = PrefabUtility.IsPartOfVariantPrefab(target);
  
            var resultAsset = PrefabUtility.GetOutermostPrefabInstanceRoot(target);
            var assetPath = string.Empty;    
            
            if (resultAsset!=null) {
                assetPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(resultAsset);
            }
            else {
                assetPath = AssetDatabase.GetAssetPath(target);
            }

            definition.AssetPath = assetPath;
            definition.InstanceStatus = instanceStatus;
            definition.PrefabAssetType = prefabAssetType;
            definition.Asset = target;
            
            definition.IsRegularPrefab = prefabAssetType != PrefabAssetType.NotAPrefab;
            definition.IsVariantPrefab = isVariant || prefabAssetType == PrefabAssetType.Variant;
            definition.IsInstance = instanceStatus == PrefabInstanceStatus.Connected;
            
            return definition;
        }

        public static void SavePrefab(this ref EditorPrefabDefinition prefabDefinition)
        {
            GameLog.Log($"SAVE PREFAB: {prefabDefinition}");
            
            //regular scene instance
            if (!prefabDefinition.SourcePrefab) {
                EditorSceneManager.SaveOpenScenes();
                return;
            }

            try {
                //prefab instance on scene
                if (prefabDefinition.IsInstance) {
                    if (prefabDefinition.IsVariantPrefab) {
                        PrefabUtility.ApplyPrefabInstance(prefabDefinition.Asset, InteractionMode.UserAction);
                    }
                    else {
                        PrefabUtility.ApplyPrefabInstance(prefabDefinition.Asset, InteractionMode.UserAction);
                    }
                }
                else {
                    PrefabUtility.SaveAsPrefabAssetAndConnect(prefabDefinition.SourcePrefab, prefabDefinition.AssetPath,InteractionMode.UserAction);
                }
            }
            catch (Exception e) {
                GameLog.LogError(e);
            }

        }
        
    }
}
