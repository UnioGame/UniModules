namespace UniModules.UniGame.GoogleSpreadsheets.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Google.Apis.Sheets.v4;
    using Google.Apis.Sheets.v4.Data;
    using UniGreenModules.UniCore.Runtime.Interfaces;

    public class GoogleSheetClient : IDisposable, IStringUnique
    {
        #region static values

        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        public static readonly string[] ReadonlyScopes = {SheetsService.Scope.SpreadsheetsReadonly};
        public static readonly string[] WriteScope = new[] {
            "https://spreadsheets.google.com/feeds",
            SheetsService.Scope.Spreadsheets,
            SheetsService.Scope.Drive,
        };

        public static readonly string ApplicationName = "UniGame Google Sheets Importer";
        public static readonly string TokenKey        = "token.json";

        #endregion

        private readonly string                                   _sheetId;
        private          SheetsService                            _service;
        private          Spreadsheet                              _spreadSheet;
        private          Dictionary<string, IList<IList<object>>> _valueCache = new Dictionary<string, IList<IList<object>>>(4);

        #region constructor

        public GoogleSheetClient(SheetsService service, string sheetId)
        {
            _sheetId = sheetId;
            // Create Google Sheets API service.
            _service = service;

            LoadSpreadSheet();
        }

        #endregion

        public string Id => _sheetId;

        public IList<Sheet> Sheets => _spreadSheet.Sheets;

        public void Reload()
        {
            _valueCache.Clear();
            LoadSpreadSheet();
        }

        public IList<IList<object>> GetData(string tableRequest)
        {
            return _valueCache.TryGetValue(tableRequest, out var result) ? result : LoadData(tableRequest);
        }

        public async Task<IList<IList<object>>> GetDataAsync(string tableRequest)
        {
            if (_valueCache.TryGetValue(tableRequest, out var result)) {
                return result;
            }

            return await LoadDataAsync(tableRequest);
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

            UpdateCache(tableRequest, values);

            return values;
        }

        public async Task<IList<IList<object>>> LoadDataAsync(string tableRequest)
        {
            if (string.IsNullOrEmpty(tableRequest))
                return null;
            // Define request parameters.
            var spreadsheetId = _sheetId;
            var targetRange   = tableRequest;
            var request       = _service.Spreadsheets.Values.Get(spreadsheetId, targetRange);

            var response = await request.ExecuteAsync();
            var values   = response.Values;

            UpdateCache(tableRequest, values);

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

        private Spreadsheet LoadSpreadSheet()
        {
            var sheetsRequest = _service.Spreadsheets.Get(Id);
            sheetsRequest.Ranges          = new List<string>();
            sheetsRequest.IncludeGridData = false;
            _spreadSheet                  = sheetsRequest.Execute();
            return _spreadSheet;
        }
    }
}