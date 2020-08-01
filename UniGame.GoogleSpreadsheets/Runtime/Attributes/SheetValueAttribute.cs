namespace UniModules.UniGame.GoogleSpreadsheets.Runtime.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Field)]
    public class SheetValueAttribute : Attribute
    {

        public string FieldName = String.Empty;
        public bool IsKey = false;
        public bool UseFieldName = false;

        /// <summary>
        /// Marl class field as Sheet sync value
        /// </summary>
        /// <param name="fieldName">Name of Table Filed</param>
        /// <param name="isKey">If true - use this field for sync items with data</param>
        public SheetValueAttribute(string fieldName = "", bool isKey = false)
        {
            UseFieldName = string.IsNullOrEmpty(fieldName);
            IsKey = isKey;
            FieldName = fieldName;
        }
        
    }
}
