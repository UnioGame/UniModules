#if ODIN_INSPECTOR
    

namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.EditorWindow
{
    using SheetsImporter;
    using Sirenix.OdinInspector;
    using Sirenix.OdinInspector.Editor;
    using UniGreenModules.UniCore.EditorTools.Editor.AssetOperations;
    using UniGreenModules.UniCore.EditorTools.Editor.Utility;
    using UnityEditor;
    using UnityEngine;

    public class GoogleSheetImporterEditorWindow : OdinEditorWindow
    {
        #region static data
        
        [MenuItem("UniGame/Google/SpreadsheetImporterWindow")]
        public static void Open()
        {
            var window = GetWindow<GoogleSheetImporterEditorWindow>();
            window.Show();
        }
        
        #endregion

        [SerializeField]
        [HideLabel]
        [InlineEditor(InlineEditorModes.FullEditor,InlineEditorObjectFieldModes.Boxed,Expanded = true)]
        public GoogleSpreadsheetImporter _googleSheetImporter;

        [Button("Reload Spreadsheets")]
        public void Reload()
        {
            _googleSheetImporter.Initialize();
        }
        
        [Button("Save")]
        public void Save()
        {
            _googleSheetImporter.MarkDirty();
        }

        #region private methods

        protected override void OnEnable()
        {
            //load importer asset
            _googleSheetImporter = AssetEditorTools.GetAsset<GoogleSpreadsheetImporter>();
            if (!_googleSheetImporter) {
                _googleSheetImporter = ScriptableObject.CreateInstance<GoogleSpreadsheetImporter>();
                _googleSheetImporter.SaveAsset(nameof(GoogleSpreadsheetImporter), GoogleSheetImporterEditorConstants.DefaultGoogleSheetImporterPath);
            }

            Reload();
            
            base.OnEnable();

        }

        #endregion
        
    }
}
#endif
