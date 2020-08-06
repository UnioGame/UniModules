namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter.Importers
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class SpreadsheetImporter<T> : SpreadsheetsAssetsImporter
        where T : SpreadsheetSerializableImporter
    {
        [SerializeReference]
        public List<T> importers = new List<T>();
    
        public override void Load()
        {
            importers.ForEach(x => x.Load());
        }

        public sealed override List<Object> Import(SpreadsheetData spreadsheetData)
        {
            var result = new List<Object>();
            
            
            foreach (var importer in OnPreImport(importers)) {
                result.AddRange(importer.Import(spreadsheetData));
            }
            
            result = OnPostImport(result).
                ToList();
            return result;
        }

        protected virtual IEnumerable<T> OnPreImport(IEnumerable<T> sourceImporters)
        {
            return sourceImporters;
        }
        
        protected virtual IEnumerable<Object> OnPostImport(IEnumerable<Object> importedAssets)
        {
            return importedAssets;
        }
    }
}