#if ODIN_INSPECTOR


namespace UniModules.UniGame.EditorTools.Editor.AssetReferences
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.EditorTools.Editor.AssetOperations.AssetReferenceTool;
    using Core.EditorTools.Editor.EditorResources;
    using Sirenix.OdinInspector;
    using Sirenix.OdinInspector.Editor;
    using UniGreenModules.UniCore.EditorTools.Editor.AssetOperations;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public class AssetInfoWindow : OdinEditorWindow
    {
        // Add menu named "My Window" to the Window menu
        [MenuItem("UniGame/Tools/Asset Info Viewer")]
        public static void Open()
        {
            var window = GetWindow<AssetInfoWindow>();
            window.titleContent = new GUIContent("Asset Info Viewer");
            window.minSize      = new Vector2(200, 30);
            window.Show();
        }

        #region inspector

        [Sirenix.OdinInspector.OnValueChanged("UpdateGuidData")]
        public string guid;
        
        [Sirenix.OdinInspector.OnValueChanged("UpdateGuidAssetData")]
        [Sirenix.OdinInspector.InlineEditor]
        [PreviewField(ObjectFieldAlignment.Center,Height = 140)]
        public Object asset;

        public string[] referenceFilters = AssetReferenceFinder.DefaultSearchTargets.ToArray();
        
        public string[] excludeReferenceSearchFilters = new string[0];
        
        [Space(6)]
        public List<ReferencesInfoData> dependencies = new List<ReferencesInfoData>();
        
        #region public methods

        [Sirenix.OdinInspector.Button]
        [Sirenix.OdinInspector.GUIColor(0.2f, 1, 0.2f)]
        public void FindDependencies()
        { 
            CleanDependencies();
            if (!asset)
                return;

            var references = AssetReferenceFinder.FindReferences(new SearchData() {
                assets                        = new[] {asset},
                excludeReferenceSearchFilters = excludeReferenceSearchFilters,
                referenceFilters              = referenceFilters,
            });
            foreach (var reference in references.referenceMap) {
                var assetItem = reference.Key;
                var referencesData = reference.Value.
                    Select(x => x.asset).
                    ToList();
                var referenceData = new ReferencesInfoData() {
                    source     = new EditorResource().Update(assetItem),
                    references = referencesData
                };
                dependencies.Add(referenceData);
            }
        }
        
        #endregion

        #endregion

        private void CleanDependencies()
        {
            dependencies.Clear();
        }
        

        private string UpdateGuidData()
        {
            UpdateView(guid);
            return guid;
        }
        
        private Object UpdateGuidAssetData()
        {
            UpdateView(asset ? AssetEditorTools.GetGUID(asset) : string.Empty );
            return asset;
        }
        
        private string UpdateView(string newGuid)
        {
            CleanDependencies();
            
            var assetPath = AssetDatabase.GUIDToAssetPath(newGuid);
            asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
            guid  = newGuid;

            return newGuid;
        }


    }
}

#endif