namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [Serializable]
    public class SheetSliceData 
    {
        public string           sheetId  = string.Empty;
        public string           keyId    = string.Empty;
        public string           keyValue = string.Empty;
        public int              index    = -1;
        public List<SheetValue> data     = new List<SheetValue>();

        public SheetValue this[string fieldName] {
            get {
                var value = data.FirstOrDefault(x =>
                    string.Equals(x.fieldName, fieldName, StringComparison.OrdinalIgnoreCase));
                return value;
            }
        }

    }
}