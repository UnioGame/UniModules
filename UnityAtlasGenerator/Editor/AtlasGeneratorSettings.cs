using UnityEngine;
using UnityEditor;
using UnityAtlasGenerator.Helper;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AtlasGeneratorSettings", menuName = "Atlas Generator/Generation Settings", order = 50)]
public class AtlasGeneratorSettings : ScriptableObject
{
    public const string kDefaultConfigObjectName = "atlasgeneratorsettings";
    public const string kDefaultPath = "Assets/Atlases/Editor/AtlasGeneratorSettings.asset";

    [Tooltip("Creates an atlas if the specified atlas doesn't exist.")]
    public bool allowAtlasCreation = false;

    [Tooltip("Rules for managing imported assets.")]
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.ListDrawerSettings(HideAddButton = false, Expanded = false, DraggableItems = true, HideRemoveButton = false)]
#endif
    public List<AtlasGeneratorRule> rules;

    [ButtonMethod]
    public void Save()
    {
        AssetDatabase.SaveAssets();
    }

    public static AtlasGeneratorSettings Instance
    {
        get
        {
            AtlasGeneratorSettings so;
            // Try to locate settings via EditorBuildSettings.
            if (EditorBuildSettings.TryGetConfigObject(kDefaultConfigObjectName, out so))
                return so;
            // Try to locate settings via path.
            so = AssetDatabase.LoadAssetAtPath<AtlasGeneratorSettings>(kDefaultPath);
            if (so != null)
                EditorBuildSettings.AddConfigObject(kDefaultConfigObjectName, so, true);
            return so;
        }
    }
}