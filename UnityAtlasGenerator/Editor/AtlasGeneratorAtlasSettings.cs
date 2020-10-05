using UnityEngine;
using UnityEditor;
using UnityAtlasGenerator.Helper;

[CreateAssetMenu(fileName = "AtlasGeneratorAtlasSettings", menuName = "Atlas Generator/Atlas Settings", order = 50)]
public class AtlasGeneratorAtlasSettings : ScriptableObject
{
    public const string kDefaultConfigObjectName = "atlasgeneratoratlassettings";
    public const string kDefaultPath = "Assets/Atlases/Editor/AtlasGeneratorAtlasSettings.asset";

    //public SpriteAtlasSettings defaultAtlasSettings;

    [ButtonMethod]
    public void Save()
    {
        AssetDatabase.SaveAssets();
    }

    public static AtlasGeneratorAtlasSettings Instance
    {
        get
        {
            AtlasGeneratorAtlasSettings so;
            // Try to locate settings via EditorBuildSettings.
            if (EditorBuildSettings.TryGetConfigObject(kDefaultConfigObjectName, out so))
                return so;
            // Try to locate settings via path.
            so = AssetDatabase.LoadAssetAtPath<AtlasGeneratorAtlasSettings>(kDefaultPath);
            if (so != null)
                EditorBuildSettings.AddConfigObject(kDefaultConfigObjectName, so, true);
            return so;
        }
    }
}
