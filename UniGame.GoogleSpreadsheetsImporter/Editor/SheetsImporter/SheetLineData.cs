namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System.Collections.Generic;

    public class SheetLineData
    {
        public string        id   = string.Empty;
        public List<object>  data = new List<object>();
        public IList<object> sourceData;
    }
}