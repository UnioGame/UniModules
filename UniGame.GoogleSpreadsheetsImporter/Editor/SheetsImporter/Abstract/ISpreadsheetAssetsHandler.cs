namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter.Abstract
{
    using System.Collections.Generic;

    public interface ISpreadsheetAssetsHandler : ISpreadsheetAssetsImporter,ISpreadsheetAssetsExporter
    {
        
        IEnumerable<object> Load();

    }
}