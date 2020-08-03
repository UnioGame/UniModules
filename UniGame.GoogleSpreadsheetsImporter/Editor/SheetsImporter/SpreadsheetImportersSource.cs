namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class SpreadsheetImportersSource : ISpreadsheetAssetsSource
    {
        /// <summary>
        /// list of assets linked by attributes
        /// </summary>
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineEditor(Sirenix.OdinInspector.InlineEditorModes.GUIOnly,
            Sirenix.OdinInspector.InlineEditorObjectFieldModes.Foldout)]
#endif
        public List<SpreadsheetsSyncAssetsImporter> importers = new List<SpreadsheetsSyncAssetsImporter>();

        public void Load()
        {
            foreach (var importer in importers) {
                importer.Load();
            }
        }

        public void Import(SpreadsheetData spreadsheetData)
        {
            foreach (var importer in importers) {
                importer.Import(spreadsheetData);
            }
        }
    }
}