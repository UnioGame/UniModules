namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class SpreadsheetImportersHandler : ISpreadsheetAssetsHandler
    {
        /// <summary>
        /// list of assets linked by attributes
        /// </summary>
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineEditor(Sirenix.OdinInspector.InlineEditorModes.GUIOnly,
            Sirenix.OdinInspector.InlineEditorObjectFieldModes.Foldout)]
#endif
        public List<SpreadsheetsSyncAssetsImporter> importers = new List<SpreadsheetsSyncAssetsImporter>();

        public IEnumerable<ISpreadsheetAssetsHandler> Importers => importers;

        public void Load()
        {
            foreach (var importer in Importers) {
                importer.Load();
            }
        }

        public void Import(SpreadsheetData spreadsheetData)
        {
            Load();
            
            foreach (var importer in Importers) {
                importer.Import(spreadsheetData);
            }
        }
    }
}