namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class SheetLineData
    {
        public string        id   = string.Empty;
        public List<object>  data = new List<object>();
        public IList<object> sourceData;
    }
}