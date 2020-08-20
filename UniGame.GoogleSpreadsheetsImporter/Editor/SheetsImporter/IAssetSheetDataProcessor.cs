namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;
    using Object = UnityEngine.Object;

    public interface IAssetSheetDataProcessor
    {
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
        List<Object> SyncFolderAssets(
            Type filterType, 
            string folder,
            bool createMissing, 
            SpreadsheetData spreadsheetData,
            int maxItems = -1,
            string overrideSheetId = "");

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
        List<Object> SyncFolderAssets(
            Type filterType, 
            string folder,
            SpreadsheetData spreadsheetData,
            Object[] assets = null,
            bool createMissing = true, 
            int maxItemsCount = -1,
            string overrideSheetId = "");

        IEnumerable<Object> ApplyAssets(
            Type filterType,
            string sheetId,
            string folder,
            SheetSyncScheme syncScheme,
            SpreadsheetData spreadsheetData,
            object[] keys,
            Object[] assets = null,
            int count = -1,
            bool createMissing = true,
            string keyFieldName = "");

        object ApplyData(object source, SpreadsheetData spreadsheetData);
        
        object ApplyData(SheetValueInfo syncValueInfo);

        bool              UpdateSheetValue(object source, SpreadsheetData data,string sheetId = "", string sheetKeyField = "");

        bool UpdateSheetValue(SheetValueInfo sheetValueInfo);
        
        IEnumerable<SyncField> SelectSheetFields(SheetSyncScheme schemaValue,SheetData data);

    }
}