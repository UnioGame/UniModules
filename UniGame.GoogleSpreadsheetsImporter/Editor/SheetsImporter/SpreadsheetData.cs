namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [Serializable]
    public class SpreadsheetData
    {
        public List<SheetData> _sheets = new List<SheetData>();

        public void Initialize(IEnumerable<SheetData> sheets)
        {
            _sheets.Clear();
            _sheets.AddRange(sheets);
        }

        public bool HasSheet(string sheetId) => _sheets.Any(x => x.Id.Equals(sheetId));
        
        public IReadOnlyList<SheetData> Sheets => _sheets;
        
        public SheetData this[string sheetName] => _sheets.FirstOrDefault(x => x.Id == sheetName);
    }
}