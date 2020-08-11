namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;

    public class SheetValue : ISheetLocation
    {
        public object value;
        public string sheetName = string.Empty;
        public string fieldName = string.Empty;
        public int row;
        public int column;

        public int Row => row;

        public int Column => column;

        public bool IsEqualField(string name)
        {
            return fieldName.Equals(name, StringComparison.OrdinalIgnoreCase);
        }
    }
}