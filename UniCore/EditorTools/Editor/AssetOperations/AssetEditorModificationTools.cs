namespace UniModule.UnityTools.EditorTools
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public partial class AssetEditorTools
    {
        #region Asset Creation/Saving

        public static TAsset SaveAsset<TAsset>(TAsset asset, string name, string folder)
            where TAsset : Object
        {
            if (folder.IndexOf('\\', folder.Length - 1) >= 0) {
                folder = folder.Remove(folder.Length - 1);
            }

            var skinTypePath = folder + "\\" + name + "." + GetAssetExtension(asset);
            var itemPath     = AssetDatabase.GenerateUniqueAssetPath(skinTypePath);

            var gameObjectAsset = asset as GameObject;
            if (gameObjectAsset != null) {
                gameObjectAsset.name = name;
                return PrefabUtility.SaveAsPrefabAssetAndConnect(gameObjectAsset, itemPath, InteractionMode.AutomatedAction) as TAsset;
            }

            AssetDatabase.CreateAsset(asset, itemPath);
            AssetDatabase.SaveAssets();
            return AssetDatabase.LoadAssetAtPath<TAsset>(itemPath);
        }

        public static bool SaveAssetAsNested(Object child, Object root, string name = null)
        {
            var assetPath = AssetDatabase.GetAssetPath(root);
            if (string.IsNullOrEmpty(assetPath))
                return false;

            var childName = name == null ? child.name : name;
            childName  = string.IsNullOrEmpty(childName) ? child.GetType().Name : childName;
            child.name = childName;

            AssetDatabase.AddObjectToAsset(child, root);

            EditorUtility.SetDirty(root);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return true;
        }

        public static Object SaveAssetAsNested(Object root, Type assetType, string name = null)
        {
            var asset  = ScriptableObject.CreateInstance(assetType);
            var result = SaveAssetAsNested(asset, root, name);
            if (result) return asset;
            return null;
        }

        public static TTarget SaveAssetAsNested<TTarget>(Object root, string name = null)
            where TTarget : ScriptableObject
        {
            var asset  = ScriptableObject.CreateInstance<TTarget>();
            var result = SaveAssetAsNested(asset, root, name);
            if (result) return asset;
            return null;
        }

        public static TAsset CreateAsset<TAsset>(string name, string folder)
            where TAsset : ScriptableObject
        {
            if (folder.IndexOf('\\', folder.Length - 1) >= 0) {
                folder = folder.Remove(folder.Length - 1);
            }

            var asset        = ScriptableObject.CreateInstance<TAsset>();
            var skinTypePath = folder + "\\" + name + ".asset";
            var itemPath     = AssetDatabase.GenerateUniqueAssetPath(skinTypePath);
            AssetDatabase.CreateAsset(asset, itemPath);

            AssetDatabase.SaveAssets();

            return asset;
        }

        [MenuItem("Assets/Set Selected Dirty")]
        public static void SetSelectedAsDirty()
        {
            var selectedFolders = GetActiveAssets();
            if (selectedFolders.Count > 0) {
                var folderAssets = GetAssets<Object>(selectedFolders.Select(x => x.assetPath).ToArray());
                foreach (var folderAsset in folderAssets) {
                    EditorUtility.SetDirty(folderAsset);
                }
            }

            var selectedItems = Selection.objects.ToList();
            foreach (var asset in selectedItems) {
                SetDirty(asset);
            }

            if (Selection.gameObjects.Length == 0) return;
            foreach (var asset in Selection.gameObjects) {
                SetDirty(asset);
            }
        }

        [MenuItem("Tools/Remove ALL modifications")]
        public static void SetAllDirty()
        {
            var assets = new List<Object>();
            assets.AddRange(GetAssets<GameObject>().OfType<Object>());
            assets.AddRange(GetAssets<ScriptableObject>().OfType<Object>());

            for (int i = 0; i < assets.Count; i++) {
                var asset    = assets[i];
                var progress = i / ((float) assets.Count);
                var canceled = EditorUtility.DisplayCancelableProgressBar(
                    string.Format("Remove Modifications [{0} of {1}] :", i, assets.Count), asset.name, progress);
                if (canceled) break;
                RemoveModifications(assets[i]);
            }

            EditorUtility.ClearProgressBar();
        }

        public static void SetDirty(Object asset)
        {
            RemoveModifications(asset);
            EditorUtility.SetDirty(asset);
        }

        public static void SetDirty<T>() where T : Object
        {
            var assets = AssetEditorTools.GetAssets<T>();
            for (int i = 0; i < assets.Count; i++) {
                var asset = assets[i];
                if (asset == null) continue;
                EditorUtility.SetDirty(asset);
            }
        }

        public static void RemoveModifications(Object asset)
        {
            if (string.IsNullOrEmpty(clientDataPath)) {
                clientDataPath    = Application.dataPath.Replace("Assets", "");
                modificationRegex = new Regex(ModificationTemplate);
            }

            var assetPath = AssetDatabase.GetAssetPath(asset);

            var ext = Path.GetExtension(assetPath);
            if (_modificationsIgnoreList.Contains(ext)) return;

            var dataPath = string.Format("{0}/{1}", clientDataPath, assetPath);

            if (File.Exists(dataPath)) {
                var text   = File.ReadAllText(dataPath);
                var result = modificationRegex.Replace(text, "");
                File.WriteAllText(dataPath, result);
            }
        }

        #endregion
    }
}