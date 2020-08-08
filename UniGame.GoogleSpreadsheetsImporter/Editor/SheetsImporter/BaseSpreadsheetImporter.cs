namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System.Collections.Generic;
    using Abstract;
    using UnityEngine;

    public abstract class BaseSpreadsheetImporter : ScriptableObject, ISpreadsheetAssetsHandler
    {
        public abstract void Load();

        public abstract List<object> Import(SpreadsheetData spreadsheetData);

        public virtual SpreadsheetData Export(SpreadsheetData data) => data;
    }
}