using UnityEngine;
#if ODIN_INSPECTOR
    

namespace UniModules.UniGame.GoogleSpreadsheets.Editor.EditorWindow
{
    using SheetsImporter;
    using Sirenix.OdinInspector.Editor;
    using UniGreenModules.UniCore.EditorTools.Editor.AssetOperations;
    using UnityEditor;

    public class GoogleSheetImporterEditorWindow : OdinEditorWindow
    {
        #region static data
        
        [MenuItem("UniGame/Google/SheetImporterWindow")]
        public static void Open()
        {
            var window = GetWindow<GoogleSheetImporterEditorWindow>();
            window.Show();
        }
        
        #endregion

        private GoogleSheetImporter _googleSheetImporter;
        
        
        #region private methods

        protected override void OnEnable()
        {
            //load importer asset
            _googleSheetImporter = AssetEditorTools.GetAsset<GoogleSheetImporter>();
            if (!_googleSheetImporter) {
                _googleSheetImporter = ScriptableObject.CreateInstance<GoogleSheetImporter>();
                _googleSheetImporter.SaveAsset(nameof(GoogleSheetImporter), GoogleSheetImporterEditorConstants.DefaultGoogleSheetImporterPath);
            }
            
            base.OnEnable();

        }

        #endregion
        
    }
}
#endif
