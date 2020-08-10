#if ODIN_INSPECTOR
    

namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.EditorWindow
{
    using Core.EditorTools.Editor.AssetOperations;
    using SheetsImporter;
    using Sirenix.OdinInspector;
    using Sirenix.OdinInspector.Editor;
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
        [InlineEditor(InlineEditorObjectFieldModes.CompletelyHidden, Expanded = true)]
        public GoogleSpreadsheetImporter _googleSheetImporter;

        
        #region private methods

        public static GoogleSpreadsheetImporter GetGoogleSpreadsheetImporter()
        {
            //load importer asset
            var importer = AssetEditorTools.GetAsset<GoogleSpreadsheetImporter>();
            if (!importer) {
                importer = ScriptableObject.CreateInstance<GoogleSpreadsheetImporter>();
                importer.SaveAsset(nameof(GoogleSpreadsheetImporter), GoogleSheetImporterEditorConstants.DefaultGoogleSheetImporterPath);
            }

            return importer;
        }
        
        protected override void OnEnable()
        {
            _googleSheetImporter = GetGoogleSpreadsheetImporter();
            _googleSheetImporter.ConnectSpreadsheets();
            base.OnEnable();
        }

        #endregion
        
    }
}
#endif
