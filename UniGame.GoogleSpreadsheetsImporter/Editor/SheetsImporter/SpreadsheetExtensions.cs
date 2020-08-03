namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using UniGreenModules.UniCore.Runtime.Utils;
    using Object = UnityEngine.Object;

    public static class SpreadsheetExtensions
    {

        private static System.Func<Object, SheetSyncItemData> _syncCache =
            MemorizeTool.Create<Object, SheetSyncItemData>(x => DefaultProcessor.CreateSyncItemData(x));
            
        public static readonly AssetSheetDataProcessor DefaultProcessor = new AssetSheetDataProcessor();

        public static SheetSyncItemData ToSpreadsheetSyncedItem(this Object asset)
        {
            return _syncCache(asset);
        }
        
        public static T ApplySpreadsheetData<T>(this T asset, SpreadsheetData data)
            where T : Object
        {
            var syncAsset = _syncCache(asset);
            DefaultProcessor.ApplyData(syncAsset,data);
            return asset;
        }

        public static object ConvertType(this object source, Type target)
        {
            if (source == null)
                return null;
            return ObjectTypeConverter.TypeConverters.ConvertValue(source, target);
        }
        
    }
}