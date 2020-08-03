namespace UniModules.UniGame.GoogleSpreadsheets.Runtime.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Field)]
    public class SheetValueAttribute : Attribute
    {

        public string dataField = String.Empty;
        public bool isKey = false;
        public bool useFieldName = false;

        /// <summary>
        /// Marl class field as Sheet sync value
        /// </summary>
        /// <param name="dataField">Name of Table Filed</param>
        /// <param name="isKey">If true - use this field for sync items with data</param>
        public SheetValueAttribute(string dataField = "", bool isKey = false)
        {
            useFieldName = string.IsNullOrEmpty(dataField);
            this.isKey = isKey;
            this.dataField = dataField;
        }
        
    }
}
