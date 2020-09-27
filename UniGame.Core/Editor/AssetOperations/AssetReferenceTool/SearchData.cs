

namespace UniModules.UniGame.Core.EditorTools.Editor.AssetOperations.AssetReferenceTool
{
        using System;
        using System.Linq;

        [Serializable]
    public class SearchData
    {
        public UnityEngine.Object[] assets = new UnityEngine.Object[0];

        public string[] assetsGuids = new string[0];
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FolderPath]
#endif
        public string[] assetFolders = new string[0];
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FolderPath]
#endif
        public string[] foldersFilter = new string[0];
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FolderPath]
#endif
        public string[] regExFilters = new string[0];
        public string[] fileTypes = AssetReferenceFinder.DefaultSearchTargets.ToArray();
    }
}