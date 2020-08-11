namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter.Abstract
{
    using UniRx;

    public interface ISpreadsheetAssetsHandler : ISpreadsheetAssetsImporter,ISpreadsheetAssetsExporter
    {
        
        void Load();

    }
}