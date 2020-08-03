namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    [Serializable]
    public class SyncItemField
    {
        private FieldInfo _targetFieldInfo;

        public readonly SheetId sheetId;
        public readonly Type   targetType;
        public readonly object target;
        public readonly string objectValueField;
        public readonly string sheetValueField;
        
        public List<SyncItemField> fields = new List<SyncItemField>();

        public SyncItemField(object target,SheetId sheetId, string objectValueField, string sheetValueField)
        {
            this.target      = target;
            this.sheetId = sheetId;
            this.objectValueField = objectValueField;
            this.sheetValueField  = sheetValueField;

            _targetFieldInfo = target.GetType().
                GetField(objectValueField, 
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            
            this.targetType = _targetFieldInfo?.FieldType;
        }

        public string SheetName => sheetId.sheetName;
        public string SheetKeyField => sheetId.keyField;
        public string SheetKeyValue => sheetId.keyValue;

        public object Value {
            get => _targetFieldInfo.GetValue(target);
            set => _targetFieldInfo.SetValue(target, value);
        }
        
    }
}