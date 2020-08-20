namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.EditorTools.Editor.AssetOperations;
    using Extensions;
    using TypeConverters.Editor;
    using UniGreenModules.UniCore.EditorTools.Editor;
    using UniGreenModules.UniGame.Core.Runtime.Extension;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public class AssetSheetDataProcessor : IAssetSheetDataProcessor
    {
        private static SheetSyncScheme _dummyItem = new SheetSyncScheme(string.Empty);

        public SheetSyncScheme CreateSyncItem(object source)
        {
            return source == null ? _dummyItem : CreateSyncItem(source.GetType());
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
            var result = SyncFolderAssets(
                filterType, 
                folder,
                spreadsheetData,
                assets.ToArray(), 
                createMissing,maxItems,overrideSheetId );
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
            Object[] assets = null,
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

            var syncScheme = filterType.CreateSheetScheme();
            
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
            
            var keysId   = keyField.sheetField;
            var column     = sheet.GetColumn(keysId);
            if (column == null) {
                Debug.LogWarning($"{nameof(AssetSheetDataProcessor)} Keys line missing with id = {keysId}");
                return result;
            }

            var updatedItems = ApplyAssets(
                filterType,
                sheetId,
                folder,
                syncScheme,
                spreadsheetData,
                sheet.GetColumnValues(keysId).ToArray(),
                assets, maxItemsCount, createMissing);
            
            result.AddRange(updatedItems);

            return result;
        }

        public IEnumerable<Object> ApplyAssets(
            Type filterType,
            string sheetId,
            string folder,
            SheetSyncScheme syncScheme,
            SpreadsheetData spreadsheetData,
            object[] keys,
            Object[] assets = null,
            int count = -1,
            bool createMissing = true,
            string keyFieldName = "")
        {
            count = count < 0 ? keys.Length : count;
            count = Math.Min(keys.Length, count);
            
            var keyField = string.IsNullOrEmpty(keyFieldName) ?
                syncScheme.keyField :
                syncScheme.GetFieldBySheetFieldName(keyFieldName);
            
            try {
                for (var i = 0; i < count; i++) {
                    
                    var keyValue = keys[i];
                    var key      = keyValue.TryConvert<string>();
                    var targetAsset = assets?.
                        FirstOrDefault(x => string.Equals(keyField.
                                GetValue(x).TryConvert<string>(), 
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

                    //show assets progression
                    AssetEditorTools.ShowProgress(new ProgressData() {
                        IsDone = false,
                        Progress = i / (float)count,
                        Content = $"{i}:{count}  {targetAsset.name}",
                        Title = "Spreadsheet Importing"
                    });
                    
                    var spreadsheetValueInfo = new SheetValueInfo() {
                        Source = targetAsset,
                        SheetId = sheetId,
                        SpreadsheetData = spreadsheetData,
                        SyncScheme = syncScheme,
                        SyncFieldName = keyField.sheetField,
                        SyncFieldValue = keyValue,
                    };
                    ApplyData(spreadsheetValueInfo);

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

        public object ApplyData(SheetValueInfo valueInfo, DataRow row)
        {
            var syncScheme      = valueInfo.SyncScheme;
            var spreadsheetData = valueInfo.SpreadsheetData;
            var source          = valueInfo.Source;
            var rowValues       = row.ItemArray;
            var table           = row.Table;
            
            for (var i = 0; i < rowValues.Length; i++) {
                var columnName = table.Columns[i].ColumnName;
                var itemField = syncScheme.fields.
                    FirstOrDefault(x => SheetData.IsEquals(x.sheetField,columnName));

                if (itemField == null)
                    continue;

                var rowValue    = rowValues[i];
                var resultValue = rowValue.ConvertType(itemField.targetType);

                itemField.ApplyValue(source, resultValue);
                
                if (itemField.IsSheetTarget) {
                    ApplyData(new SheetValueInfo() {
                        Source          = resultValue,
                        SpreadsheetData = spreadsheetData,
                        SyncScheme      = itemField.targetType.CreateSheetScheme(),
                    });
                }
            }

            return source;
        }

        public object ApplyData(object source, SpreadsheetData spreadsheetData)
        {
            var syncScheme = source.CreateSheetScheme();
            
            var syncValue = new SheetValueInfo() {
                Source          = source,
                SpreadsheetData = spreadsheetData,
                SheetId         = syncScheme.sheetId,
                SyncScheme      = syncScheme,
                SyncFieldName  = syncScheme.keyField.sheetField,
            };
            
            var result     = ApplyData(syncValue);
            return result;
        }

        public bool ValidateSheetInfo(ref SheetValueInfo sheetValueInfo)
        {
            var source          = sheetValueInfo.Source;
            var syncScheme      = sheetValueInfo.SyncScheme;
            var spreadsheetData = sheetValueInfo.SpreadsheetData;
            var sheetId         = sheetValueInfo.SheetId;

            syncScheme = syncScheme ?? source?.GetType().CreateSheetScheme();

            if (source == null || syncScheme == null)
                return false;
            
            sheetId = string.IsNullOrEmpty(sheetValueInfo.SheetId) ? 
                syncScheme.sheetId : sheetId;
            sheetValueInfo.SheetId = sheetId;

            if (!spreadsheetData.HasSheet(sheetId))
                return false;
            
            var keyField =  string.IsNullOrEmpty(sheetValueInfo.SyncFieldName) ? 
                syncScheme.keyField?.sheetField : 
                sheetValueInfo.SyncFieldName ;

            if (string.IsNullOrEmpty(keyField) || !spreadsheetData.HasSheet(sheetId))
                return false;
                
            sheetValueInfo.SyncFieldName = keyField;

            var syncKeyField = syncScheme.GetFieldBySheetFieldName(keyField);
            var keyValue     = sheetValueInfo.SyncFieldValue ?? syncKeyField?.GetValue(source);
            if (keyValue == null)
                return false;
            
            sheetValueInfo.SyncFieldValue = keyValue;

            return true;
        }
        
        public object ApplyData(SheetValueInfo sheetValueInfo)
        {
            if (!ValidateSheetInfo(ref sheetValueInfo))
                return sheetValueInfo.Source;
            
            var spreadsheetData = sheetValueInfo.SpreadsheetData;
            var sheetId         = sheetValueInfo.SheetId;

            var sheet = spreadsheetData[sheetId];
            var row   = sheet.GetRow(sheetValueInfo.SyncFieldName, sheetValueInfo.SyncFieldValue);
            
            var result = ApplyData(sheetValueInfo, row);
            return result;
        }

        public bool UpdateSheetValue(object source, SpreadsheetData data,string sheetId = "", string sheetKeyField = "")
        {
            if (source == null)
                return false;
            var type       = source.GetType();
            var syncScheme = type.CreateSheetScheme();

            var keyField = string.IsNullOrEmpty(sheetKeyField) ?
                syncScheme.keyField :
                syncScheme.GetFieldBySheetFieldName(sheetKeyField);
            
            if (keyField == null)
                return false;
            
            var keyValue = keyField.GetValue(source);
            
            var sheetValueInfo = new SheetValueInfo() {
                Source = source,
                SpreadsheetData = data,
                SyncScheme = syncScheme,
                SyncFieldName = sheetKeyField,
                SheetId = sheetId
            };
            
            return UpdateSheetValue(sheetValueInfo);
        }
        
        public bool UpdateSheetValue(SheetValueInfo sheetValueInfo)
        {
            if (!ValidateSheetInfo(ref sheetValueInfo))
                return false;
            
            var source          = sheetValueInfo.Source;
            var schemeValue = sheetValueInfo.SyncScheme;
            var spreadsheetData = sheetValueInfo.SpreadsheetData;
            var sheetId         = sheetValueInfo.SheetId;

            var sheet = spreadsheetData[sheetId];
            var row   = sheet.GetRow(sheetValueInfo.SyncFieldName, sheetValueInfo.SyncFieldValue) ?? sheet.CreateRow();
            
            //var sheetFields = SelectSheetFields(schemaValue, data);
            var fields = schemeValue.fields;

            for (var i = 0; i < fields.Length; i++) {
                var field       = fields[i];
                var sourceValue = field.GetValue(source);
                sourceValue = sourceValue ?? string.Empty;
                sheet.UpdateValue(row, field.sheetField, sourceValue);

                if (field.IsSheetTarget) {
                     
                }
            }

            return true;
        }

        public IEnumerable<SyncField> SelectSheetFields(SheetSyncScheme schemaValue,SheetData data)
        {
            var columns = data.Columns;
            for (var i = 0; i < columns.Count; i++) {
                var column = columns[i];
                var field = schemaValue.fields.
                    FirstOrDefault(x => SheetData.
                        IsEquals(x.sheetField, column.ColumnName));
                if(field == null)
                    yield return null;
                yield return field;
            }
        }

    }
}