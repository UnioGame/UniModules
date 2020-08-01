namespace UniModules.UniGame.GoogleSpreadsheets.Editor.SheetsImporter
{
    using System.Collections.Generic;

    public class SheetSyncItemData
    {
        public readonly object target;
        
        public SyncItemField       keyField;
        public List<SyncItemField> fields = new List<SyncItemField>();

        public SheetSyncItemData(object value)
        {
            target = value;
        }
    }
}