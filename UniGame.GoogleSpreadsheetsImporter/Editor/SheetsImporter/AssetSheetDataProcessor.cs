namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using GoogleSpreadsheets.Editor.SheetsImporter;
    using GoogleSpreadsheets.Runtime.Attributes;
    using UniGreenModules.UniCore.Runtime.ReflectionUtils;
    using UnityEngine;

    public class AssetSheetDataProcessor
    {
        private static SheetSyncItemData _dummyItem = new SheetSyncItemData(string.Empty);
        
        public SheetSyncItemData CreateSyncItemData(object target)
        {
            if (target == null)
                return _dummyItem;

            var type         = target.GetType();
            var sheetName    = type.Name;
            var useAllFields = false;
            var keyField     = GoogleSheetImporterConstants.KeyField;
            var keyValue     = string.Empty;
            
            var result             = new SheetSyncItemData(target);
            var sheetItemAttribute = type.GetCustomAttribute<SpreadsheetTargetAttribute>();
            if (sheetItemAttribute != null) {
                keyField     = sheetItemAttribute.KeyField;
                useAllFields = sheetItemAttribute.SyncAllFields;
                sheetName    = sheetItemAttribute.SheetName;
            }
            
            var sheetId = new SheetId() {
                keyField  = keyField,
                keyValue  = keyValue,
                sheetName = sheetName,
            };

            var fields = type.GetInstanceFields();
            foreach (var fieldInfo in fields) {

                var customAttribute = fieldInfo.GetType().
                    GetCustomAttribute<SheetValueAttribute>();
                if(customAttribute == null && !useAllFields)
                    continue;
                
                var fieldValue = fieldInfo.GetValue(target);
                var fieldName  = fieldInfo.Name;
                var sheetField = fieldName;
                var isKeyField = false;

                if (customAttribute != null) {
                    sheetField = string.IsNullOrEmpty(customAttribute.dataField) ? sheetField : customAttribute.dataField;
                    isKeyField = customAttribute.isKey;
                }

                if (string.Equals(keyField, fieldName, StringComparison.OrdinalIgnoreCase)) {
                    sheetId.keyValue = fieldValue.ToString();
                }
                
                if (isKeyField) {
                    sheetId.keyField = sheetField;
                    sheetId.keyValue = fieldValue.ToString();
                }

                var syncField = new SyncItemField(target, sheetId, fieldName, sheetField);

                result.fields.Add(syncField);
            }
            
            return result;

        }
        
        public void ApplyData(SheetSyncItemData target, SpreadsheetData spreadsheetData)
        {
            
            foreach (var itemField in target.fields) {

                var sheet = spreadsheetData[itemField.SheetName];
                if (sheet == null) {
                    Debug.LogWarning($"{nameof(AssetSheetDataProcessor)}: Missing sheet with name {itemField.SheetName}");
                    continue;
                }

                var slice = sheet.GetSliceByKeyValue(itemField.SheetKeyField, itemField.SheetKeyValue);
                var fieldData = slice[itemField.sheetValueField];

                if(fieldData == null)
                    continue;
                
                var resultValue = fieldData.value.ConvertType(itemField.targetType);
                itemField.Value = resultValue;

            }
        }

    }
}