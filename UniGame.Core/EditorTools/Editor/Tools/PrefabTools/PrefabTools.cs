namespace UniGreenModules.UniCore.EditorTools.Editor.PrefabTools
{
    using System;
    using System.Diagnostics;
    using Runtime.ProfilerTools;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;
    using Debug = UnityEngine.Debug;

    public static class PrefabTools 
    {
        public static bool ApplyComponent(this GameObject target, Component component)
        {
            var definition = target.GetPrefabDefinition();
            if (string.IsNullOrEmpty(definition.AssetPath))
                return false;
            PrefabUtility.ApplyAddedComponent(component,definition.AssetPath,InteractionMode.UserAction);
            return true;
        }

        public static EditorPrefabDefinition GetPrefabDefinition(this Component component)
        {
            return GetPrefabDefinition(component.gameObject);
        }


        [Conditional("UNITY_EDITOR")]
        public static void SaveScenes(this object source)
        {
            EditorSceneManager.SaveOpenScenes();
        }
        
        [Conditional("UNITY_EDITOR")]
        public static void SaveAll(this object source)
        {
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();
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
                    prefabDefinition.Asset = PrefabUtility.SaveAsPrefabAssetAndConnect(prefabDefinition.SourcePrefab, prefabDefinition.AssetPath,InteractionMode.UserAction);
                }
            }
            catch (Exception e) { 
                Debug.LogError($"CANT SAVE \n {prefabDefinition}");
                GameLog.LogError(e);
            }

        }
        
        public static GameObject SaveAndConnectPrefab(this ref EditorPrefabDefinition prefabDefinition)
        {
            GameLog.Log($"SAVE PREFAB: {prefabDefinition}");
            
            //regular scene instance
            if (!prefabDefinition.SourcePrefab) {
                EditorSceneManager.SaveOpenScenes();
                return prefabDefinition.Asset;
            }

            try {
                //prefab instance on scene
                if (prefabDefinition.IsInstance) {
                    if (prefabDefinition.IsVariantPrefab) {
                        PrefabUtility.ApplyPrefabInstance(prefabDefinition.Asset, InteractionMode.UserAction);
                    }
                    else {
                        prefabDefinition.Asset = PrefabUtility.SaveAsPrefabAssetAndConnect(prefabDefinition.Asset, prefabDefinition.AssetPath,InteractionMode.UserAction);
                    }
                }
            }
            catch (Exception e) {
                GameLog.LogError(e);
            }

            return prefabDefinition.Asset;
        }
        
    }
}
