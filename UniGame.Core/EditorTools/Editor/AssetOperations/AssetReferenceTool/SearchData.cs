namespace UniModules.UniGame.EditorTools.Editor.AssetReferences
{
    using System;
    using System.Linq;
    using Core.EditorTools.Editor.AssetOperations.AssetReferenceTool;

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
        public string[] excludeReferenceSearchFilters = new string[0];
        public string[] referenceFilters = AssetReferenceFinder.DefaultSearchTargets.ToArray();
    }
}