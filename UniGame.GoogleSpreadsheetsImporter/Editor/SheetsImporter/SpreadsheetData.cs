namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [Serializable]
    public class SpreadsheetData
    {
        public List<SheetData> _sheets = new List<SheetData>();
        
        public void Initialize(List<GoogleSpreadsheetClient> clients)
        {
            _sheets.Clear();
            _sheets.AddRange(clients.SelectMany(x => x.GetAllSheetsData()));
        }

        public SheetData this[string sheetName] => _sheets.FirstOrDefault(x => x.Id == sheetName);
    }
}