namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter.Importers
{
    using System;
    using System.Collections.Generic;
    using Abstract;
    using Object = UnityEngine.Object;

    [Serializable]
    public abstract class SpreadsheetSerializableImporter : ISpreadsheetAssetsHandler
    {
        public abstract List<Object> Import(SpreadsheetData spreadsheetData);

        public abstract void Load();
    }
}