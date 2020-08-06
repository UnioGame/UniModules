namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System.Collections.Generic;
    using Abstract;
    using UnityEngine;

    public abstract class SpreadsheetsAssetsImporter : ScriptableObject, ISpreadsheetAssetsHandler
    {
        public abstract void Load();

        public abstract List<Object> Import(SpreadsheetData spreadsheetData);
        
    }
}