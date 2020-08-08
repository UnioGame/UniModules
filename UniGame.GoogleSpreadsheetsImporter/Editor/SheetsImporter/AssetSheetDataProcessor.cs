namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.EditorTools.Editor.AssetOperations;
    using Extensions;
    using GoogleSpreadsheets.Editor.SheetsImporter;
    using GoogleSpreadsheets.Runtime.Attributes;
    using UniGreenModules.UniCore.EditorTools.Editor;
    using UniGreenModules.UniCore.Runtime.ReflectionUtils;
    using UniGreenModules.UniGame.Core.Runtime.Extension;
    using UnityEditor;
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
            var useAllFields = true;

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
        /// <param name="maxItems"></param>
        /// <param name="overrideSheetId"></param>
        /// <returns></returns>
        public List<Object> SyncFolderAssets(
            Type filterType, 
            string folder,
            bool createMissing, 
            SpreadsheetData spreadsheetData,
            int maxItems = -1,
            string overrideSheetId = "")
        {
            if (!filterType.IsScriptableObject() && !filterType.IsComponent()) {
                Debug.LogError($"SyncFolderAssets: BAD target type {filterType}");
                return null;
            }
            
            var assets = AssetEditorTools.GetAssets<Object>(filterType, folder);
            var result = SyncFolderAssets(filterType, folder,spreadsheetData,assets, createMissing,maxItems,overrideSheetId );
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
        /// <param name="maxItemsCount"></param>
        /// <param name="overrideSheetId">force override target sheet id</param>
        /// <returns></returns>
        public List<Object> SyncFolderAssets(
            Type filterType, 
            string folder,
            SpreadsheetData spreadsheetData,
            List<Object> assets = null,
            bool createMissing = true, 
            int maxItemsCount = -1,
            string overrideSheetId = "")
        {
            var result = assets != null ? 
                new List<Object>(assets) : 
                new List<Object>();
            
            if (!filterType.IsScriptableObject() && !filterType.IsComponent()) {
                Debug.LogError($"SyncFolderAssets: BAD target type {filterType}");
                return result;
            }

            var syncScheme = filterType.ToSpreadsheetSyncedItem();
            
            var sheetId = string.IsNullOrEmpty(overrideSheetId) ?
                syncScheme.sheetId : 
                overrideSheetId;
            
            var sheet      = spreadsheetData[sheetId];
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
            var keys     = sheet.GetData(keysId);
            if (keys == null) {
                Debug.LogWarning($"{nameof(AssetSheetDataProcessor)} Keys line missing with id = {keysId}");
                return result;
            }

            foreach (var importedAsset in ApplyAssets(filterType,sheetId,folder,syncScheme,spreadsheetData,keys,assets,maxItemsCount,createMissing)) {
                result.Add(importedAsset);
            }

            return result;
        }

        public IEnumerable<Object> ApplyAssets(Type filterType,
            string sheetId,
            string folder,
            SheetSyncValue syncScheme,
            SpreadsheetData spreadsheetData,
            SheetLineData keys,
            List<Object> assets = null,
            int count = -1,
            bool createMissing = true)
        {
            count = count < 0 ? keys.data.Count : count;
            count = Math.Min(keys.data.Count, count);
            
            var keyField = syncScheme.keyField;
            
            try {
                for (var i = 0; i < count; i++) {
                    
                    var keyValue = keys.data[i];
                    var key      = keyValue.value as string;
                    var targetAsset = assets?.
                        FirstOrDefault(x => string.Equals(keyField.
                                GetValue(x).ToString(),
                            key, StringComparison.OrdinalIgnoreCase));

                    //create asset if missing
                    if (targetAsset == null) {
                        //skip asset creation step
                        if (createMissing == false)
                            continue;

                        targetAsset = filterType.CreateAsset();
                        targetAsset.SaveAsset($"{filterType.Name}_{i+1}", folder,false);
                        Debug.Log($"Create Asset [{targetAsset}] for path {folder}", targetAsset);
                    }

                    AssetEditorTools.ShowProgress(new ProgressData() {
                        IsDone = false,
                        Progress = i / (float)count,
                        Content = $"{i}:{count}  {targetAsset.name}",
                        Title = "Spreadsheet Importing"
                    });
                    
                    ApplyData(targetAsset, key, sheetId, syncScheme, spreadsheetData);

                    yield return targetAsset;
                }
            }
            finally {
                AssetEditorTools.ShowProgress(new ProgressData() {
                    IsDone = true,
                });
                AssetDatabase.SaveAssets();
            }

        }
        
        
        public object ApplyData(object source,SheetSyncValue syncScheme, IEnumerable<SheetValue> slice)
        {
            var sheetValues = slice as SheetValue[] ?? slice.ToArray();
            foreach (var itemField in syncScheme.fields) {
                
                var sheetValue = sheetValues.
                    FirstOrDefault(x => x.IsEqualField(itemField.sheetValueField));

                if(sheetValue == null)
                    continue;
                
                var resultValue = sheetValue.value.ConvertType(itemField.targetType);
                
                itemField.ApplyValue(source, resultValue);
            }

            return source;
        }
        
        public object ApplyDataByAssetKey(object source,SheetSyncValue value, SpreadsheetData spreadsheetData)
        {
            var keyField = value.keyField;
            var keyValue = keyField.GetValue(source);

            return ApplyData(source, keyValue,value.sheetId, value, spreadsheetData);
        }
        
        public SheetData UpdateSheetValue(object source, SheetData data)
        {
            if (source == null)
                return data;
            var type = source.GetType();
            var syncScheme = type.ToSpreadsheetSyncedItem();

            var keyField = syncScheme.keyField;
            if (keyField == null)
                return data;
            
            var keyValue = keyField.GetValue(source);
            
            return UpdateSheetValue(source,keyValue,syncScheme,data);
        }
        
        public SheetData UpdateSheetValue(object source,object keyValue, SheetSyncValue schemaValue, SheetData data)
        {
            if (keyValue == null || source == null)
                return data;
            
            var keyField = schemaValue.keyField;
            var key = keyField.sheetValueField;
            foreach (var field in schemaValue.fields) {
                var sheetField = field.sheetValueField;
                var sheetValue = data.GetValue(key, keyField.GetValue(source), sheetField);
                if(sheetValue == null)
                    continue;
                var value = field.GetValue(source);
                data.UpdateValue(value,sheetValue.row,sheetValue.column);
            }

            return data;
        }


        public object ApplyData(object source,object key,string sheetId,SheetSyncValue value, SpreadsheetData spreadsheetData)
        {
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

                var fieldName  = fieldInfo.Name.TrimStart('_');
                var sheetField = customAttribute!=null && !customAttribute.useFieldName ? 
                    customAttribute.dataField : fieldName;
                
                var isKeyField = string.Equals(keyFieldName, fieldName, StringComparison.OrdinalIgnoreCase);
                var syncField = new SyncField(fieldInfo, sheetField,isKeyField);

                yield return syncField;
            }
        }

    }
}