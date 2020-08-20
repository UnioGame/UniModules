namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter.Extensions
{
    using System;
    using System.Collections.Generic;
    using TypeConverters.Editor;
    using Object = UnityEngine.Object;

    public static class SpreadsheetExtensions
    {
        
        public static readonly AssetSheetDataProcessor DefaultProcessor = new AssetSheetDataProcessor();

        public static bool UpdateSheetValue(this object source, SpreadsheetData data,string sheetId,string sheetKeyField)
        {
            return DefaultProcessor.UpdateSheetValue(source, data,sheetId,sheetKeyField);
        }
        
        public static bool UpdateSheetValue(this object source, SpreadsheetData data)
        {
            return DefaultProcessor.UpdateSheetValue(source, data);
        }
        
        public static bool UpdateSheetValue(this object source, SpreadsheetData data,string sheetId)
        {
            return DefaultProcessor.UpdateSheetValue(source, data,sheetId);
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
        
        public static object ApplySpreadsheetData(
            this object asset,
            SpreadsheetData spreadsheetData, 
            string sheetId,
            object keyValue = null,
            string sheetFieldName = "")
        {
            if (spreadsheetData.HasSheet(sheetId) == false)
                return asset;
            
            var syncAsset = asset.CreateSheetScheme();

            var sheetValueIndo = new SheetValueInfo() {
                Source = asset,
                SheetId = sheetId,
                SpreadsheetData = spreadsheetData,
                SyncScheme = syncAsset,
                SyncFieldName = sheetFieldName,
                SyncFieldValue = keyValue,
            };
            
            return DefaultProcessor.ApplyData(sheetValueIndo);
        }

        public static object ApplySpreadsheetData(this object asset, SpreadsheetData data)
        {
            return DefaultProcessor.ApplyData(asset,data);
        }
        
        public static object ApplySpreadsheetData(this object asset,SheetSyncScheme syncAsset, SpreadsheetData data)
        {
            var sheetValueInfo = new SheetValueInfo() {
                Source = asset,
                SyncScheme = syncAsset,
                SpreadsheetData = data
            };
            return DefaultProcessor.ApplyData(sheetValueInfo);
        }

        public static object ApplySpreadsheetData(
            this object asset,
            Type type,
            SpreadsheetData sheetData,
            string sheetId,
            object keyValue = null,
            string sheetFieldName = "")
        {
            var syncAsset = type.CreateSheetScheme();
            
            var sheetValue = new SheetValueInfo() {
                Source = asset,
                SheetId = sheetId,
                SpreadsheetData = sheetData,
                SyncScheme = syncAsset,
                SyncFieldName = sheetFieldName,
                SyncFieldValue = keyValue,
            };
            
            var result = DefaultProcessor.ApplyData(sheetValue);
            
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