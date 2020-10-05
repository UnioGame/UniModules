using UnityEngine;
using UnityEditor;
using UnityAtlasGenerator.Helper;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "AtlasGeneratorAtlasSettings", menuName = "Atlas Generator/Atlas Settings", order = 50)]
public class AtlasGeneratorAtlasSettings : ScriptableObject
{
    public const string kDefaultConfigObjectName = "atlasgeneratoratlassettings";
    public const string kDefaultPath = "Assets/Atlases/Editor/AtlasGeneratorAtlasSettings.asset";

    [Tooltip("Atlas used when no atlas path is specified.")]
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.AssetsOnly]
#endif
    public SpriteAtlas DefaultAtlas;

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

    public SpriteAtlas GetDefaultAtlas()
    {
        if (DefaultAtlas != null)
            return DefaultAtlas;

        var defaultAtlasPath = "Assets/GameContent/Atlases/DefaultAtlas.spriteatlas";
        var atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(defaultAtlasPath);
        if (atlas == null)
        {
            AssetDatabase.CreateAsset(new SpriteAtlas(), defaultAtlasPath);
            return AssetDatabase.LoadAssetAtPath<SpriteAtlas>(defaultAtlasPath);
        }
        return atlas;
    }
}
