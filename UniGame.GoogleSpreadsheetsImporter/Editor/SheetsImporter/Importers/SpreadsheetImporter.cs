namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter.Importers
{
    using System.Collections.Generic;
    using System.Linq;

    public class SpreadsheetImporter<T> : BaseSpreadsheetImporter
        where T : SpreadsheetSerializableImporter
    {
        public  List<T> importers = new List<T>();
    
        public override IEnumerable<object> Load()
        {
            var result  = new List<object>();
            foreach (var x in importers) {
                result.AddRange(x.Load());
            }
            return result;
        }

        public override SpreadsheetData Export(SpreadsheetData data)
        {
            foreach (var importer in importers) {
                importer.Export(data);
            }

            return data;
        }

        public sealed override List<object> Import(SpreadsheetData spreadsheetData)
        {
            var result = new List<object>();
            
            foreach (var importer in OnPreImport(importers)) {
                result.AddRange(importer.Import(spreadsheetData));
            }
            
            result = OnPostImport(result).ToList();
            return result;
        }

        protected virtual IEnumerable<T> OnPreImport(IEnumerable<T> sourceImporters)
        {
            return sourceImporters;
        }
        
        protected virtual IEnumerable<object> OnPostImport(IEnumerable<object> importedAssets)
        {
            return importedAssets;
        }
    }
}