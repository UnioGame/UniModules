namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter.Importers
{
    using System;
    using System.Collections.Generic;
    using UniGreenModules.UniCore.EditorTools.Editor.AssetOperations;
    using UnityEditor;
    using Object = UnityEngine.Object;

    [Serializable]
    public class AssetFolderImporter : SerializableSpreadsheetImporter
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.VerticalGroup("Filter")]
        [Sirenix.OdinInspector.FolderPath(RequireExistingPath = true)]
        [Sirenix.OdinInspector.LabelWidth(50)]
        [Sirenix.OdinInspector.Required]
#endif
        public string folder;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.VerticalGroup("Filter")]
        [Sirenix.OdinInspector.LabelWidth(50)]
        [Sirenix.OdinInspector.LabelText("Type")]
        [Sirenix.OdinInspector.Required]
#endif
        public MonoScript assetTypeScript;


#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.VerticalGroup("Filter")]
        [Sirenix.OdinInspector.LabelWidth(100)]
        [Sirenix.OdinInspector.LabelText("Create Missing")]
#endif
        public bool createMissingItems;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineEditor()]
#endif
        public List<Object> values = new List<Object>();

        public string Folder => folder;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button()]
#endif
        public override void Load()
        {
            values.Clear();
            if (string.IsNullOrEmpty(folder) || assetTypeScript == null)
                return;

            var filterType = assetTypeScript.GetClass();
            if (filterType == null)
                return;

            values = AssetEditorTools.GetAssets<Object>(filterType, new[] {folder});
        }

        public override void Import(SpreadsheetData spreadsheetData)
        {
            var filterType = assetTypeScript.GetClass();
            if (filterType == null)
                return;

            var syncedAsset = filterType.SyncFolderAssets(folder,createMissingItems, spreadsheetData);
            
        }
        
    }
}