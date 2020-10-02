using System.IO;
using UnityEditor;
using UnityEngine;

public class AtlasCleaner : UnityEditor.AssetModificationProcessor
{
    public static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
    {
        if (AssetDatabase.GetMainAssetTypeAtPath(assetPath) == typeof(Texture2D) && Path.GetExtension(assetPath.ToLower()) != ".psb")
        {
            AtlasGenerator.RemoveSpriteFromAtlas(assetPath);
        }

        return AssetDeleteResult.DidNotDelete;
    }
}