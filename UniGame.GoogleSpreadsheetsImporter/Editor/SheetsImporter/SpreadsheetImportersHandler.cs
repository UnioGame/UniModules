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
        public List<BaseSpreadsheetImporter> importers = new List<BaseSpreadsheetImporter>();

        public IEnumerable<ISpreadsheetAssetsHandler> Importers => importers;

        public void Load()
        {
            foreach (var importer in Importers) {
                importer.Load();
            }
        }

        public List<object> Import(SpreadsheetData spreadsheetData)
        {
            var result = new List<object>();
            foreach (var importer in Importers) {
                importer.Load();
                var imported = importer.Import(spreadsheetData);
                result.AddRange(imported);
            }

            return result;
        }

        public SpreadsheetData Export(SpreadsheetData data)
        {
            foreach (var importer in importers) {
                importer.Load();
                data = importer.Export(data);
            }

            return data;
        }
    }
}