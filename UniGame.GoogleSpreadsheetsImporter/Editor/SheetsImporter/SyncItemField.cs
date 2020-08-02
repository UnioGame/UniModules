namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using System.Reflection;

    public class SyncItemField
    {
        private FieldInfo _targetFieldInfo;

        public readonly Type   targetType;
        public readonly object target;
        public readonly string objectField;
        public readonly string sheetField;

        public SyncItemField(object target, string objectField, string sheetField)
        {
            this.target      = target;
            this.objectField = objectField;
            this.sheetField  = sheetField;
            this.targetType  = target.GetType();

            _targetFieldInfo = targetType.GetField(objectField, 
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            
            
        }

        public object Value {
            get => _targetFieldInfo.GetValue(target);
            set => _targetFieldInfo.SetValue(target, value);
        }
        
    }
}