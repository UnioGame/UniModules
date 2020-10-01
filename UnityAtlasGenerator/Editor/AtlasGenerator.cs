using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
public class AtlasGenerator : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        var generationSettings = AtlasGeneratorSettings.Instance;
        if (generationSettings == null)
        {
            Debug.LogWarningFormat("[AtlasGenerator] generation settings file not found.\nPlease go to Assets/Atlases/Editor folder, right click in the project window and choose 'Create > Atlas Generator > Generation Settings'.");
            return;
        }/*
        if (generationSettings.rules == null || generationSettings.rules.Count == 0)
            return;
        var dirty = false;*/
    }
}