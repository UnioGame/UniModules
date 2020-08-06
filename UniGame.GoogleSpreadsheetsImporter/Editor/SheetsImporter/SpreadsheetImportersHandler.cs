namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;
    using Abstract;
    using Object = UnityEngine.Object;

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
        public List<SpreadsheetsAssetsImporter> importers = new List<SpreadsheetsAssetsImporter>();

        public IEnumerable<ISpreadsheetAssetsHandler> Importers => importers;

        public void Load()
        {
            foreach (var importer in Importers) {
                importer.Load();
            }
        }

        public List<Object> Import(SpreadsheetData spreadsheetData)
        {
            Load();
            
            var result = new List<Object>();
            foreach (var importer in Importers) {
                var imported = importer.Import(spreadsheetData);
                result.AddRange(imported);
            }

            return result;
        }
    }
}