namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class SheetSyncItemData
    {
        public readonly object target;
        
        public List<SyncItemField> fields = new List<SyncItemField>();

        public SheetSyncItemData(object value)
        {
            target = value;
        }
    }
}