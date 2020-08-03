namespace UniModules.UniGame.GoogleSpreadsheets.Runtime.Attributes
{
    using System;
    using Editor.SheetsImporter;

    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Field)]
    public class SpreadsheetTargetAttribute : Attribute
    {
        private string _sheetName = string.Empty;
        private string _range = string.Empty;
        private bool _syncAllFields = false;
        private string _keyField = GoogleSheetImporterConstants.KeyField;

        public SpreadsheetTargetAttribute(string sheetName = "",string keyField = "",string range = "", bool syncAllFields = true)
        {
            _sheetName = sheetName;
            _range = range;
            _keyField = string.IsNullOrEmpty( keyField ) ? _keyField : keyField;
            _syncAllFields = syncAllFields;
        }

        public bool UseTypeName => string.IsNullOrEmpty(_sheetName);

        public string SheetName => _sheetName;

        public string Range => _range;

        public string KeyField => _keyField;

        public bool SyncAllFields => _syncAllFields;
    }
}
