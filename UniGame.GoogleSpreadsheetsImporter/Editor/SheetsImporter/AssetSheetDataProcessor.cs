namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.EditorTools.Editor.AssetOperations;
    using Extensions;
    using GoogleSpreadsheets.Editor.SheetsImporter;
    using GoogleSpreadsheets.Runtime.Attributes;
    using UniGreenModules.UniCore.Runtime.ReflectionUtils;
    using UniGreenModules.UniCore.Runtime.Utils;
    using UniGreenModules.UniGame.Core.Runtime.Extension;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public class AssetSheetDataProcessor
    {
        private static SheetSyncValue _dummyItem = new SheetSyncValue(string.Empty);

        public SheetSyncValue CreateSyncItem(object source)
        {
            return source == null ? _dummyItem : CreateSyncItem(source.GetType());
        }
        
        public SheetSyncValue CreateSyncItem(Type type)
        {
            var sheetName    = type.Name;
            var useAllFields = false;

            var sheetItemAttribute = type.GetCustomAttribute<SpreadsheetTargetAttribute>();
            if (sheetItemAttribute != null) {
                useAllFields = sheetItemAttribute.SyncAllFields;
                sheetName    = sheetItemAttribute.SheetName;
            }

            var result = new SheetSyncValue(sheetName);

            var fields = LoadSyncFieldsData(type, useAllFields);
            result.fields = fields.ToArray();
            
            result.keyField = result.fields.FirstOrDefault(x => x.isKeyField);
            
            return result;

        }

        /// <summary>
        /// Sync folder assets by spreadsheet data
        /// </summary>
        /// <param name="filterType"></param>
        /// <param name="folder"></param>
        /// <param name="createMissing">if true - create missing assets</param>
        /// <param name="spreadsheetData"></param>
        /// <returns></returns>
        public List<Object> SyncFolderAssets(
            Type filterType, 
            string folder,
            bool createMissing, 
            SpreadsheetData spreadsheetData)
        {
            if (!filterType.IsScriptableObject() && !filterType.IsComponent()) {
                Debug.LogError($"SyncFolderAssets: BAD target type {filterType}");
                return null;
            }
            
            var assets = AssetEditorTools.GetAssets<Object>(filterType, folder);
            var result = SyncFolderAssets(filterType, assets, folder, createMissing, spreadsheetData);
            return result;
        }

        /// <summary>
        /// Sync folder assets by spreadsheet data
        /// </summary>
        /// <param name="filterType"></param>
        /// <param name="assets"></param>
        /// <param name="folder"></param>
        /// <param name="createMissing">if true - create missing assets</param>
        /// <param name="spreadsheetData"></param>
        /// <returns></returns>
        public List<Object> SyncFolderAssets(
            Type filterType, 
            List<Object> assets,
            string folder,
            bool createMissing, 
            SpreadsheetData spreadsheetData)
        {
            var result = new List<Object>(assets);
            
            if (!filterType.IsScriptableObject() && !filterType.IsComponent()) {
                Debug.LogError($"SyncFolderAssets: BAD target type {filterType}");
                return result;
            }

            var syncScheme = filterType.ToSpreadsheetSyncedItem();
            var sheetId    = syncScheme.sheetId;
            var sheet      = spreadsheetData[syncScheme.sheetId];
            if (sheet == null) {
                Debug.LogWarning($"{nameof(AssetSheetDataProcessor)} Missing Sheet with name {sheetId}");
                return result;
            }

            var keyField = syncScheme.keyField;

            if (keyField == null) {
                Debug.LogWarning($"{nameof(AssetSheetDataProcessor)} Key field missing sheet = {sheetId}");
                return result;
            }
            
            var keysId   = keyField.sheetValueField;
            var keys     = sheet.GetLine(keysId);
            if (keys == null) {
                Debug.LogWarning($"{nameof(AssetSheetDataProcessor)} Keys line missing with id = {keysId}");
                return result;
            }

            foreach (var keyValue in keys.data) {
                var key = keyValue as string;
                var targetAsset = assets.FirstOrDefault(
                    x => string.Equals(keyField.GetValue(x).ToString(),
                        key, StringComparison.OrdinalIgnoreCase));
                
                //create asset if missing
                if (targetAsset == null ) {
                    //skip asset creation step
                    if(createMissing == false)
                        continue;
                    
                    targetAsset = filterType.CreateAsset();
                    targetAsset.SaveAsset($"{sheetId}_{key}", folder);
                    Debug.Log($"Create Asset [{targetAsset}] for path {folder}",targetAsset);
                }
 
                ApplyData(targetAsset,key, syncScheme, spreadsheetData);
                
                result.Add(targetAsset);
            }

            return result;
        }
        
        public object ApplyData(object source,SheetSyncValue value, SheetSliceData slice)
        {
            foreach (var itemField in value.fields) {

                var fieldData = slice[itemField.sheetValueField];

                if(fieldData == null)
                    continue;
                
                
                var resultValue = fieldData.value.ConvertType(itemField.targetType);
                
                itemField.ApplyValue(source, resultValue);
            }

            return source;
        }
        
        public object ApplyDataByAssetKey(object source,SheetSyncValue value, SpreadsheetData spreadsheetData)
        {
            var keyField = value.keyField;
            var keyValue = keyField.GetValue(source);

            return ApplyData(source,keyValue, value, spreadsheetData);
        }
        
        public object ApplyData(object source,object key,SheetSyncValue value, SpreadsheetData spreadsheetData)
        {
            var sheetId  = value.sheetId;
            var keyField = value.keyField;
            
            if (string.IsNullOrEmpty(sheetId)) {
                Debug.LogWarning($"ApplyData SheetSyncValue : for {value.target} Key Field pr SheetId is Missing [KEY = {keyField} , SHEET_ID = {sheetId}]");
                return source;
            }
            
            var sheet    = spreadsheetData[sheetId];
                            
            if (sheet == null) {
                Debug.LogWarning($"{nameof(AssetSheetDataProcessor)}: Missing sheet with name {sheetId}");
                return source;
            }
            var slice = sheet.GetSliceByKeyValue(keyField.sheetValueField, key);

            return ApplyData(source, value, slice);
        }

        private IEnumerable<SyncField> LoadSyncFieldsData(Type sourceType, bool useAllFields)
        {
            var fields = sourceType.GetInstanceFields();

            var spreadsheetTargetAttribute = sourceType.
                GetCustomAttribute<SpreadsheetTargetAttribute>();
            
            var filedsAttributes = new List<SheetValueAttribute>();
            var keyFieldSheetName = GoogleSheetImporterConstants.KeyField;
            var keyFieldName = spreadsheetTargetAttribute != null ? 
                spreadsheetTargetAttribute.KeyField :
                GoogleSheetImporterConstants.KeyField;
            
            foreach (var field in fields) {
                var attributeInfo = field.
                    FieldType.
                    GetCustomAttribute<SheetValueAttribute>();
                filedsAttributes.Add(attributeInfo);
                if (attributeInfo != null && attributeInfo.isKey)
                    keyFieldName = attributeInfo.useFieldName ? field.Name : attributeInfo.dataField;
            }

            for (var i = 0; i < fields.Count; i++)
            {
                var fieldInfo       = fields[i];
                var customAttribute = filedsAttributes[i];
                if (customAttribute == null && !useAllFields)
                    continue;

                var fieldName  = fieldInfo.Name;
                var sheetField = customAttribute!=null && !customAttribute.useFieldName ? 
                    customAttribute.dataField : fieldName;
                
                var isKeyField = string.Equals(keyFieldName, fieldName, StringComparison.OrdinalIgnoreCase);
                var syncField = new SyncField(fieldInfo, sheetField,isKeyField);

                yield return syncField;
            }
        }

    }
}