namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter.Importers
{
    using System;
    using Abstract;

    [Serializable]
    public abstract class SerializableSpreadsheetImporter : ISpreadsheetAssetsHandler
    {
        public abstract void Import(SpreadsheetData spreadsheetData);

        public abstract void Load();
    }
}