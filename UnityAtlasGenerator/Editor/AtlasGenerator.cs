using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine.U2D;
using UnityEditor.U2D;
using System.Linq;
using System;

public class AtlasGenerator : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        var generatorSettings = AtlasGeneratorSettings.Instance;
        if (generatorSettings == null)
        {
            Debug.LogWarningFormat("[AtlasGenerator] generation settings file not found.\nPlease go to Assets/Atlases/Editor folder, right click in the project window and choose 'Create > Atlas Generator > Generation Settings'.");
            return;
        }
        var atlasSettings = AtlasGeneratorAtlasSettings.Instance;
        if (atlasSettings == null)
        {
            Debug.LogWarningFormat("[AtlasGenerator] settings file not found.\nPlease go to Assets/Atlases/Editor folder, right click in the project window and choose 'Create > Atlas Generator > Atlas Settings'.");
            return;
        }
        if (generatorSettings.rules == null || generatorSettings.rules.Count == 0)
            return;

        var dirty = false;

        // Apply generation rules.
        foreach (var importedAsset in importedAssets)
        {
            if (AssetDatabase.GetMainAssetTypeAtPath(importedAsset) == typeof(Texture2D) && Path.GetExtension(importedAsset.ToLower()) != ".psb")
            {
                dirty |= ApplyGenerationRule(importedAsset, null, generatorSettings, atlasSettings);
            }
        }

        for (var i = 0; i < movedAssets.Length; i++)
        {
            var movedAsset = movedAssets[i];
            var movedFromAssetPath = movedFromAssetPaths[i];
            if (AssetDatabase.GetMainAssetTypeAtPath(movedAsset) == typeof(Texture2D) && Path.GetExtension(movedAsset.ToLower()) != ".psb")
            {
                dirty |= ApplyGenerationRule(movedAsset, movedFromAssetPath, generatorSettings, atlasSettings);
            }
        }

        if (dirty)
        {
            AssetDatabase.SaveAssets();
        }
    }
    static bool ApplyGenerationRule(
       string assetPath,
       string movedFromAssetPath,
       AtlasGeneratorSettings generatorSettings,
       AtlasGeneratorAtlasSettings atlasSettings)
    {
        var dirty = false;
        if (TryGetMatchedRule(assetPath, generatorSettings, out var matchedRule))
        {
            if (movedFromAssetPath != null && TryGetMatchedRule(movedFromAssetPath, generatorSettings, out var oldMatchedRule))
            {
                var newAtlasPath = matchedRule.GetFullPathToAtlas(matchedRule.ParseAtlasReplacement(assetPath));
                var oldAtlasPath = oldMatchedRule.GetFullPathToAtlas(oldMatchedRule.ParseAtlasReplacement(movedFromAssetPath));
                if (newAtlasPath != oldAtlasPath)
                {
                    RemoveFromAtlas(matchedRule, movedFromAssetPath, assetPath, atlasSettings);
                }
            }
            // Apply the matched rule.
            var atlas = CreateOrUpdateAtlas(generatorSettings, atlasSettings, matchedRule, assetPath);
            if (atlas != null)
            {
                Debug.LogFormat("[AtlasGenerator] Added sprite {0} to atlas {1}", assetPath, atlas.name);
            }

            dirty = true;
        }
        else
        {
            // If assetPath doesn't match any of the rules, try to remove the sprite.
            // But only if movedFromAssetPath has the matched rule, because the generator should not remove any unmanaged sprites.
            if (!string.IsNullOrEmpty(movedFromAssetPath) && TryGetMatchedRule(movedFromAssetPath, generatorSettings, out matchedRule))
            {
                if (RemoveFromAtlas(matchedRule, movedFromAssetPath, assetPath, atlasSettings))
                {
                    dirty = true;
                }
            }
        }

        return dirty;
    }

    static SpriteAtlas CreateOrUpdateAtlas(
        AtlasGeneratorSettings generatorSettings,
        AtlasGeneratorAtlasSettings atlasSettings,
        AtlasGeneratorRule rule,
        string assetPath)
    {
        // Set atlas
        SpriteAtlas atlas;
        var pathToAtlas = rule.ParseAtlasReplacement(assetPath);
        pathToAtlas = rule.GetFullPathToAtlas(pathToAtlas);
        bool newAtlas = false;
        if (!TryGetAtlas(pathToAtlas, atlasSettings, out atlas))
        {
            atlas = CreateAtlas(pathToAtlas);
            newAtlas = true;
        }

        // Set atlas settings from template if necessary
        /*if (rule.atlasTemplate != null && (newAtlas || rule.groupTemplateApplicationMode == AtlasTemplateApplicationMode.AlwaysOverwriteAtlasSettings))
        {
            rule.atlasTemplate.ApplyToAtlas(atlas);
        }*/
        
        var packedAsset = new Texture2D[] { AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath) };
        var packedAssets = atlas.GetPackables();
        if (!packedAssets.Contains(packedAsset[0]))
        {
            atlas.Add(packedAsset);
        }

        return atlas;
    }

    static SpriteAtlas CreateAtlas(string pathToAtlas)
    {
        var atlasTemplate = new SpriteAtlas();
        var parentFolder = Path.GetDirectoryName(pathToAtlas);
        if (!AssetDatabase.IsValidFolder(parentFolder))
        {
            AssetDatabase.CreateFolder(Path.GetDirectoryName(parentFolder), Path.GetFileName(parentFolder));
        }
        AssetDatabase.CreateAsset(atlasTemplate, pathToAtlas);
        var atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(pathToAtlas);
        return atlas;
    }

    static bool RemoveFromAtlas(
        AtlasGeneratorRule rule,
        string movedFromAssetPath,
        string assetPath,
        AtlasGeneratorAtlasSettings atlasSettings)
    {
        var pathToAtlas = rule.ParseAtlasReplacement(movedFromAssetPath);
        pathToAtlas = rule.GetFullPathToAtlas(pathToAtlas);
        if (!TryGetAtlas(pathToAtlas, atlasSettings, out var atlas))
        {
            Debug.LogWarningFormat("[AtlasGenerator] Failed to find atlas {0} when removing {1} from it", rule.pathToAtlas, assetPath);
            return false;
        }
        var packedAsset = new Texture2D[] { AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath) };
        atlas.Remove(packedAsset);
        AssetDatabase.SaveAssets();
        Debug.LogFormat("[AtlasGenerator] Removed {0} from atlas", assetPath);
        var packables = atlas.GetPackables();
        if (packables.Length == 0)
        {
            AssetDatabase.DeleteAsset(pathToAtlas);
            return true;
        }

        return false;
    }

    static bool RemoveFromAtlas(
        AtlasGeneratorRule rule,
        string assetPath,
        AtlasGeneratorAtlasSettings atlasSettings)
    {
        var pathToAtlas = rule.ParseAtlasReplacement(assetPath);
        pathToAtlas = rule.GetFullPathToAtlas(pathToAtlas);
        if (!TryGetAtlas(pathToAtlas, atlasSettings, out var atlas))
        {
            Debug.LogWarningFormat("[AtlasGenerator] Failed to find atlas {0} when removing {1} from it", rule.pathToAtlas, assetPath);
            return false;
        }
        var assetToDelete = new Texture2D[] { AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath) };
        atlas.Remove(assetToDelete);
        AssetDatabase.SaveAssets();
        Debug.LogFormat("[AtlasGenerator] Removed {0} from atlas", assetPath);
        var packables = atlas.GetPackables();
        if (packables.Length == 0)
        {
            AssetDatabase.DeleteAsset(pathToAtlas);
            return true;
        }

        return false;
    }

    static bool TryGetMatchedRule(
        string assetPath,
        AtlasGeneratorSettings importSettings,
        out AtlasGeneratorRule rule)
    {
        foreach (var r in importSettings.rules)
        {
            if (!r.Match(assetPath))
                continue;
            rule = r;
            return true;
        }

        rule = null;
        return false;
    }

    /// <summary>
    /// Attempts to get the atlas using the provided <paramref name="pathToAtlas"/>.
    /// </summary>
    /// <param name="pathToAtlas">The name of the atlas for the search.</param>
    /// <param name="atlas">The <see cref="SpriteAtlas"/> if found. Set to <see cref="null"/> if not found.</param>
    /// <returns>True if a atlas is found.</returns>
    static bool TryGetAtlas(string pathToAtlas, AtlasGeneratorAtlasSettings atlasSettings, out SpriteAtlas atlas)
    {
        if (string.IsNullOrWhiteSpace(pathToAtlas))
        {
            atlas = atlasSettings.DefaultAtlas;
            return true;
        }
        return ((atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(pathToAtlas)) == null) ? false : true;
    }

    public static void RemoveSpriteFromAtlas(string assetPath)
    {
        var generatorSettings = AtlasGeneratorSettings.Instance;
        if (generatorSettings == null)
        {
            Debug.LogWarningFormat("[AtlasGenerator] generation settings file not found.\nPlease go to Assets/Atlases/Editor folder, right click in the project window and choose 'Create > Atlas Generator > Generation Settings'.");
            return;
        }
        var atlasSettings = AtlasGeneratorAtlasSettings.Instance;
        if (atlasSettings == null)
        {
            Debug.LogWarningFormat("[AtlasGenerator] settings file not found.\nPlease go to Assets/Atlases/Editor folder, right click in the project window and choose 'Create > Atlas Generator > Atlas Settings'.");
            return;
        }
        if (generatorSettings.rules == null || generatorSettings.rules.Count == 0)
            return;

        var dirty = false;

        if (TryGetMatchedRule(assetPath, generatorSettings, out var matchedRule))
        {
            if (RemoveFromAtlas(matchedRule, assetPath, atlasSettings))
            {
                dirty = true;
            }
        }
        if (dirty)
        {
            AssetDatabase.SaveAssets();
        }
    }
}