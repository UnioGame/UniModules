namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System.Collections.Generic;

    public class SheetSliceData
    {
        public string           sheetId  = string.Empty;
        public string           keyId    = string.Empty;
        public string           keyValue = string.Empty;
        public int              index    = -1;
        public List<SheetValue> data     = new List<SheetValue>();
    }
}