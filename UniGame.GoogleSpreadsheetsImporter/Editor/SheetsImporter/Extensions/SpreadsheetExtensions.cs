namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter.Extensions
{
    using System;
    using System.Collections.Generic;
    using TypeConverters.Editor;
    using Object = UnityEngine.Object;

    public static class SpreadsheetExtensions
    {
        
        public static readonly AssetSheetDataProcessor DefaultProcessor = new AssetSheetDataProcessor();

        public static SheetData UpdateSheetValue(this object source, SheetData data,string sheetKeyField)
        {
            return DefaultProcessor.UpdateSheetValue(source, data,sheetKeyField);
        }
        
        public static SheetData UpdateSheetValue(this object source, SpreadsheetData data)
        {
            var syncScheme = source.CreateSheetScheme();
            return DefaultProcessor.UpdateSheetValue(source, data);
        }
        
        public static SheetData UpdateSheetValue(this object source, SheetData data)
        {
            return DefaultProcessor.UpdateSheetValue(source, data);
        }

        public static List<Object> SyncFolderAssets(
            this Type filterType, 
            string folder,
            SpreadsheetData spreadsheetData,
            Object[] assets = null,
            bool createMissing = true, 
            int maxItemsCount = -1,
            string overrideSheetId = "")
        {
            return DefaultProcessor.SyncFolderAssets(filterType,folder, spreadsheetData,assets, createMissing,maxItemsCount,overrideSheetId);
        }
        
        public static List<Object> SyncFolderAssets(
            this Type type, 
            string folder,
            bool createMissing, 
            SpreadsheetData spreadsheetData)
        {
            return DefaultProcessor.SyncFolderAssets(type, folder,createMissing, spreadsheetData);
        }
        
        public static object ApplySpreadsheetData(this object asset,SpreadsheetData spreadsheetData, string sheetId)
        {
            if (!spreadsheetData.HasSheet(sheetId))
                return asset;

            var syncAsset = asset.CreateSheetScheme();

            var sheetValueIndo = new SheetValueInfo() {
                Source = asset,
                SheetId = sheetId,
                SpreadsheetData = spreadsheetData,
                SyncScheme = syncAsset,
            };
            
            return DefaultProcessor.ApplyData(sheetValueIndo);
        }

        public static object ApplySpreadsheetData(this object asset, SpreadsheetData data)
        {
            var syncAsset = asset.CreateSheetScheme();
            var keyField  = syncAsset.sheetId;
            
            return DefaultProcessor.ApplyDataByAssetKey(asset,syncAsset,data[keyField]);
        }
        
        public static object ApplySpreadsheetData(this object asset,SheetSyncScheme syncAsset, SpreadsheetData data)
        {
            var sheetId  = syncAsset.sheetId;
            var keyField = syncAsset.keyField;
            
            return DefaultProcessor.ApplyDataByAssetKey(asset,syncAsset,data[sheetId],keyField.sheetField);
        }

        public static object ApplySpreadsheetData(
            this object asset,
            object keyValue,
            SheetData sheetData,
            string sheetKeyName = "")
        {
            return ApplySpreadsheetData(asset, asset.GetType(), keyValue, sheetData, sheetKeyName);
        }

        public static object ApplySpreadsheetData(
            this object asset,
            Type type,
            object keyValue,
            SheetData sheetData,
            string sheetKeyName = "")
        {
            var syncAsset = type.CreateSheetScheme();
            var keyField = string.IsNullOrEmpty(sheetKeyName) ? 
                syncAsset.keyField : 
                syncAsset.GetFieldBySheetFieldName(sheetKeyName);
            
            var result = DefaultProcessor.ApplyData(
                asset,
                keyField.sheetField,
                keyValue,
                syncAsset,sheetData);
            
            return result;
        }

        public static object ConvertType(this object source, Type target)
        {
            if (source == null)
                return null;
            
            if (target.IsInstanceOfType(source))
                return source;

            return ObjectTypeConverter.TypeConverters.ConvertValue(source, target);
        }
        
    }
}