namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter.Importers.Serializable
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Core.EditorTools.Editor.AssetOperations;
    using Extensions;
    using Object = UnityEngine.Object;

    [Serializable]
    public class FolderAssetsImporter : SpreadsheetSerializableImporter
    {
        private const int LabelWidth = 120;
        
        private List<Object> _values = new List<Object>();

        protected List<Object> Values {
            get => _values = _values == null ? new List<Object>() : _values;
            set => _values = value;
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.VerticalGroup("Filter")]
        [Sirenix.OdinInspector.FolderPath(RequireExistingPath = true)]
        [Sirenix.OdinInspector.LabelWidth(LabelWidth)]
        [Sirenix.OdinInspector.Required]
#endif
        public string folder;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.VerticalGroup("Filter")]
        [Sirenix.OdinInspector.LabelWidth(LabelWidth)]
        [Sirenix.OdinInspector.LabelText("RegEx Filter")]
#endif
        public string assetRegexFilter = String.Empty;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.VerticalGroup("Filter")]
        [Sirenix.OdinInspector.LabelWidth(LabelWidth)]
        [Sirenix.OdinInspector.LabelText("Create Missing")]
#endif
        public bool createMissingItems;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.VerticalGroup("Filter")]
        [Sirenix.OdinInspector.LabelWidth(LabelWidth)]
#endif
        public bool overrideSheetId = false;
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.VerticalGroup("Filter")]
        [Sirenix.OdinInspector.LabelWidth(LabelWidth)]
        [Sirenix.OdinInspector.ShowIf("overrideSheetId")]
#endif
        public string sheetId;
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.LabelWidth(LabelWidth)]
        [Sirenix.OdinInspector.VerticalGroup("Filter")]
#endif
        public int maxItemsCount = -1;
        

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button()]
#endif
        public override void Load()
        {
            Values.Clear();
            var filterType = GetFilteredType();
            if (string.IsNullOrEmpty(folder) || filterType == null)
                return;

            Values = AssetEditorTools.GetAssets<Object>(filterType, new[] {folder});
            Values = ApplyRegExpFilter(Values);
        }

        public sealed override List<object> Import(SpreadsheetData spreadsheetData)
        {
            var result = new List<object>();
            var filterType = GetFilteredType();
            if (filterType == null)
                return result;

            var syncedAsset = filterType.SyncFolderAssets(
                folder,
                spreadsheetData,
                Values.ToArray(),
                createMissingItems, 
                maxItemsCount,
                overrideSheetId ? sheetId : string.Empty);
            
            result.AddRange(OnPostImportAction(syncedAsset));

            return result;
        }

        protected virtual Type GetFilteredType() => typeof(Object);

        protected virtual IEnumerable<Object> OnPostImportAction(IEnumerable<Object> importedAssets)
        {
            return importedAssets;
        }
        
        private List<Object> ApplyRegExpFilter(List<Object> assets)
        {
            if (string.IsNullOrEmpty(assetRegexFilter))
                return assets;
            
            var filteredAssets = new List<Object>();
            var regexpr = new Regex(assetRegexFilter, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            
            foreach (var asset in assets) {
                if (!asset) continue;
                var resource = asset.ToEditorResource();
                if(!regexpr.IsMatch(resource.AssetPath))
                    continue;
                filteredAssets.Add(asset);
            }

            return filteredAssets;
        }
        
    }
}