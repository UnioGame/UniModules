#if ODIN_INSPECTOR
    

namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.EditorWindow
{
    using SheetsImporter;
    using Sirenix.OdinInspector;
    using Sirenix.OdinInspector.Editor;
    using UniGreenModules.UniCore.EditorTools.Editor.AssetOperations;
    using UnityEditor;
    using UnityEngine;

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

        [SerializeField]
        [HideLabel]
        [InlineEditor(InlineEditorModes.FullEditor,InlineEditorObjectFieldModes.Boxed,Expanded = true)]
        public GoogleSheetImporter _googleSheetImporter;

        [Button]
        public void Reload()
        {
            _googleSheetImporter.Initialize();
        }

        #region private methods

        protected override void OnEnable()
        {
            //load importer asset
            _googleSheetImporter = AssetEditorTools.GetAsset<GoogleSheetImporter>();
            if (!_googleSheetImporter) {
                _googleSheetImporter = ScriptableObject.CreateInstance<GoogleSheetImporter>();
                _googleSheetImporter.SaveAsset(nameof(GoogleSheetImporter), GoogleSheetImporterEditorConstants.DefaultGoogleSheetImporterPath);
            }

            Reload();
            
            base.OnEnable();

        }

        #endregion
        
    }
}
#endif
