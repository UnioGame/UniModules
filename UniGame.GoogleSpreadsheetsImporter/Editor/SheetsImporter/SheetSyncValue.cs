namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class SheetSyncValue
    {
        public object target;

        public string sheetId;

        public SyncField keyField;
        
        public SyncField[] fields = new SyncField[0];

        public SheetSyncValue(string sheetId)
        {
            this.sheetId = sheetId;
        }
    }
}