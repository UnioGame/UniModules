namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [Serializable]
    public class SheetSyncScheme
    {
        public object target;

        public string sheetId;

        public SyncField keyField;
        
        public SyncField[] fields = new SyncField[0];

        public SheetSyncScheme(string sheetId)
        {
            this.sheetId = sheetId;
        }

        public SyncField GetFieldBySheetFieldName(string fieldName)
        {
            return fields.FirstOrDefault(x => SheetData.IsEquals(x.sheetField, fieldName));
        }
        
        public SyncField GetFieldByObjectField(string fieldName)
        {
            return fields.FirstOrDefault(x => SheetData.IsEquals(x.objectField, fieldName));
        }
    }
}