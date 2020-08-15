namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter.Abstract
{
    using System.Collections.Generic;
    using UnityEngine;

    public interface ISpreadsheetAssetsImporter
    {
        List<object> Import(SpreadsheetData spreadsheetData);
    }
}