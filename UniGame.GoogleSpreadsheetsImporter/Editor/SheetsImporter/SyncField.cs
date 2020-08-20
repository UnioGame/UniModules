namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    [Serializable]
    public class SyncField
    {
        private FieldInfo _fieldInfo;
        public readonly Type   targetType;
        public readonly string objectField;
        public readonly string sheetField;
        public readonly bool isKeyField;
        public readonly List<SyncField> fields = new List<SyncField>();

        public SyncField(FieldInfo field, string sheetValueField,bool isKeyField)
        {
            _fieldInfo = field;
            this.objectField = _fieldInfo.Name;
            this.sheetField  = sheetValueField.TrimStart('_');
            this.isKeyField = isKeyField;
            this.targetType = _fieldInfo.FieldType;
        }

        public object GetValue(object source)
        {
            return _fieldInfo.GetValue(source);
        }

        public SyncField ApplyValue(object source, object value)
        {
            _fieldInfo.SetValue(source,value);
            return this;
        }

        public override string ToString()
        {
            return $"Object Field: {objectField} IsKey: {isKeyField} Sheet Field: {sheetField} TargetType: {targetType.Name}";
        }
    }
}