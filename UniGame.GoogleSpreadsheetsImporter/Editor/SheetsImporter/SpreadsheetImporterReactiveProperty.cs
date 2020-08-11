namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using Abstract;
    using UniRx;

    [Serializable]
    public class SpreadsheetImporterReactiveProperty : ReactiveProperty<ISpreadsheetAssetsHandler>
    {
    }
}