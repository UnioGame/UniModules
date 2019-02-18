using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Editor.Utility
{
    public static class EditorAssetUtility
    {
        /// <summary>
        //	This makes it easy to create, name and place unique new ScriptableObject asset files.
        /// </summary>
        public static Object CreateAsset(Type type, string path)
        {
            var asset = ScriptableObject.CreateInstance(type);
            return SaveAsset(asset, path,"");
        }

        public static Object SaveAsAsset(Object asset,string name,string extension)
        {
            var path = EditorUtility.SaveFilePanelInProject("Save As", name, extension, "");
            AssetDatabase.DeleteAsset(path);
            var assetObject = Object.Instantiate(asset);
            return SaveAsset(assetObject, Path.GetDirectoryName(path), 
                Path.GetFileNameWithoutExtension(path));
        }

        public static string GetUniqueFolderName(string asset,string path)
        {
            var resultPath = Path.GetDirectoryName(path);
            resultPath += "/" + asset;
            if (!Directory.Exists(resultPath))
                return asset;
            for (int i = 0; i < 10000; i++)
            {
                var tempPath = resultPath + i;
                if (!Directory.Exists(tempPath))
                    return asset + i;
            }
            return null;
        }

        public static Object SaveAssetRecursiveToUniqueFolder(Object asset,
            string folderName, string path,Type[] ignoreTypes = null)
        {
            var folder = GetUniqueFolderName(folderName, path);
            var targetPath = Path.GetDirectoryName(path);
            var uniquePath = AssetDatabase.CreateFolder(targetPath, folder);
            var newFolderPath = AssetDatabase.GUIDToAssetPath(uniquePath);
            return SaveAsAssetRecursive(asset, newFolderPath,Path.GetFileName(newFolderPath), ignoreTypes);
        }

        public static Object SaveAsAssetRecursive(Object asset, string path, 
            string name,Type[] ignoreTypes = null)
        {
            if (!asset) return null;
            var result = SaveAsset(Object.Instantiate(asset), path, name);
            var fieldValues = result.GetType().GetFields();
            foreach (var field in fieldValues)
            {
                var value = field.GetValue(result);
                var targetAsset = value as ScriptableObject;
                if (targetAsset == null)
                {
                    SaveAssetList<ScriptableObject>(value as IList, path, ignoreTypes);
                    continue;
                }
                var assetType = targetAsset.GetType();
                if (ignoreTypes != null && ignoreTypes.Any(x => x.IsAssignableFrom(assetType)))
                {
                    Debug.Log($"PREVENT SAVE ASSET {assetType}");
                    continue;
                }
                var resultFieldAsset = SaveAsAssetRecursive(targetAsset, path, 
                    targetAsset.name,ignoreTypes);
                field.SetValue(result, resultFieldAsset);
            }
            return result;
        }

        public static void SaveAssetList<TData>(IList items,string path,Type[] ignoreAssets = null)
            where TData : Object
        {
            if (items == null)
                return;
            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var targetAsset = item as TData;
                if (!targetAsset)
                    continue;
                var createdItem = SaveAsAssetRecursive(targetAsset, path,
                    targetAsset.name, ignoreAssets) as TData;
                items[i] = createdItem;
            }
        }

        public static Object SaveAsset(Object asset,string path,string name,string extension = "")
        {
            var type = asset.GetType();
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.
                    GetAssetPath(Selection.activeObject)), "");
            }
            var fileName = string.IsNullOrEmpty(name) ? type.Name : name;
            var fileExtension = string.IsNullOrEmpty(extension) ? "asset" : extension;
            var resultPath = $"{path}/{fileName}.{fileExtension}";
            var assetPathAndName =
                AssetDatabase.GenerateUniqueAssetPath(resultPath);
            var message = string.Format("SAVE asset {0} as PATH : {1}", 
                fileName, assetPathAndName);
            Debug.Log(message);
            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return AssetDatabase.LoadAssetAtPath(assetPathAndName, type);
        }

        public static Object[] LoadAssets(Type objectType,string name = null)
        {
            var searchName = string.IsNullOrEmpty(name) ? string.Empty : name;
            var searchId = $"t: {objectType.Name} {searchName}";
            var foundObjects = AssetDatabase
                .FindAssets(searchId)
                .Select(AssetDatabase.GUIDToAssetPath);
            var objects = foundObjects as string[] ?? foundObjects.ToArray();
            var assets = objects.Select(_ => AssetDatabase.LoadAssetAtPath(_, objectType));
            var foundAssets = assets.Where(where => string.IsNullOrEmpty(name) || 
                string.Compare(where.name, name, StringComparison.OrdinalIgnoreCase) == 0);
            return foundAssets.ToArray();
        }

        public static TData[] LoadAssets<TData>(string name = null)
        {
            var assets = LoadAssets(typeof(TData), name).OfType<TData>();
            return assets.ToArray();
        }
    }
}