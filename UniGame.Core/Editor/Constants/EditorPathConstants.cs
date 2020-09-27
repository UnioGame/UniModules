namespace UniModules.UniGame.Core.EditorTools.Editor
{
    using System.IO;
    using AssetOperations;
    using Core.Editor.Constants;
    using UnityEditor;

    public class EditorPathConstants
    {
        public const string GeneratedContentDefaultPath = "Assets/UniGame.Generated/";
        
        public static string GeneratedContentPath {
            get
            {
                var marker = AssetEditorTools.LoadOrCreate<UniGenerationPathMarker>(GeneratedContentDefaultPath);
                if (!marker) {
                    return GeneratedContentDefaultPath;
                }
                var path = AssetDatabase.GetAssetPath(marker);
                return Path.GetDirectoryName(path);
            }
        }
    }
}
