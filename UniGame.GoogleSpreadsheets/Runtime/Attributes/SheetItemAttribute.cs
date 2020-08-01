namespace UniModules.UniGame.GoogleSpreadsheets.Runtime.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class SheetItemAttribute : Attribute
    {
        private string _sheetName = string.Empty;
        private string _range = string.Empty;
        private bool _syncAllFields = false;
        private string _keyField = String.Empty;

        public SheetItemAttribute(string sheetName = "",string range = "",string keyField = "", bool syncAllFields = false)
        {
            _sheetName = sheetName;
            _range = range;
            _keyField = keyField;
            _syncAllFields = syncAllFields;
        }

        public bool UseTypeName => string.IsNullOrEmpty(_sheetName);

        public string SheetName => _sheetName;

        public string Range => _range;

        public string KeyField => _keyField;

        public bool SyncAllFields => _syncAllFields;
    }
}
