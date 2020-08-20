namespace UniModules.UniGame.GoogleSpreadsheets.Runtime.Attributes
{
    public interface ISpreadsheetDescription
    {
        bool   UseTypeName   { get; }
        string SheetName     { get; }
        string KeyField      { get; }
        bool   SyncAllFields { get; }
    }
}