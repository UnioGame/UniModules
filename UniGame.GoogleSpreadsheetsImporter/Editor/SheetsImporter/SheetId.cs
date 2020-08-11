namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using GoogleSpreadsheets.Editor.SheetsImporter;

    [Serializable]
    public class SheetId
    {
        public string sheetName = string.Empty;
        public string keyField = GoogleSheetImporterConstants.KeyField;
    }
}