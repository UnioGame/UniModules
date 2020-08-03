namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System.Collections.Generic;

    public interface ISpreadsheetAssetsImporter
    {
        void Import(SpreadsheetData spreadsheetData);
    }
}