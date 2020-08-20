namespace UniModules.UniGame.GoogleSpreadsheets.Runtime.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Field)]
    public class SheetFieldAttribute : Attribute
    {
        public string dataField = String.Empty;
        public bool isKey = false;
        public bool useFieldName = false;

        /// <summary>
        /// Marl class field as Sheet sync value
        /// </summary>
        /// <param name="dataField">Name of Table Filed</param>
        /// <param name="isKey">If true - use this field for sync items with data</param>
        public SheetFieldAttribute(string dataField = "", bool isKey = false)
        {
            this.useFieldName = string.IsNullOrEmpty(dataField);
            this.isKey = isKey;
            this.dataField = dataField;
        }
        
    }
}
