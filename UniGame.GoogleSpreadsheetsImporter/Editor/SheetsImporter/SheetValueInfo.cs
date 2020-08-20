namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    public struct SheetValueInfo
    {
        public object          Source;
        public SheetSyncScheme SyncScheme;
        public SpreadsheetData SpreadsheetData;
        public string          SheetId;
        public string          SyncFieldName;
        public object          SyncFieldValue;
    }
}