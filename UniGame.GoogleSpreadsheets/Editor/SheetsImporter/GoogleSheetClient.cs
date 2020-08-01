namespace UniModules.UniGame.GoogleSpreadsheets.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Google.Apis.Sheets.v4;

    public class GoogleSheetClient : IDisposable
    {
        #region static values

        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        public static readonly string[] Scopes          = { SheetsService.Scope.SpreadsheetsReadonly };
        public static readonly string   ApplicationName = "UniGame Google Sheets Importer";
        public static readonly string   TokenKey        = "token.json";

        #endregion
        
        private readonly string _sheetId;
        private SheetsService _service;
        private string _currentRange = string.Empty;
        private Dictionary<string,IList<IList<object>>> _valueCache = new Dictionary<string, IList<IList<object>>>(4);
        

        public GoogleSheetClient(SheetsService service,string sheetId)
        {
            _sheetId = sheetId;
            // Create Google Sheets API service.
            _service = service;
        }

        public IList<IList<object>> LoadData(string tableRequest)
        {
            if (string.IsNullOrEmpty(tableRequest))
                return null;
            // Define request parameters.
            var spreadsheetId = _sheetId;
            var targetRange   = tableRequest;
            var request       = _service.Spreadsheets.Values.Get(spreadsheetId, targetRange);

            var response = request.Execute();
            var values   = response.Values;

            _valueCache[tableRequest] = values;
            
            return values;
        }
        
        public async Task<IList<IList<object>>> LoadDataAsync(string tableRequest)
        {
            if (string.IsNullOrEmpty(tableRequest))
                return null;
            // Define request parameters.
            var spreadsheetId = _sheetId;
            var targetRange   = tableRequest;
            var request       = _service.Spreadsheets.
                Values.Get(spreadsheetId, targetRange);

            var response = await request.ExecuteAsync();
            var values   = response.Values;

            _valueCache[tableRequest] = values;
            
            return values;
        }

        public void Dispose()
        {
            _service?.Dispose();
        }

        private void UpdateCache(string tableRequest, IList<IList<object>> values)
        {
            _valueCache[tableRequest] = values;
        }
    }
}
