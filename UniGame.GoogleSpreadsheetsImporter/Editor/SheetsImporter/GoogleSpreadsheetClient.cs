namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using Google.Apis.Sheets.v4;
    using Google.Apis.Sheets.v4.Data;
    using Sirenix.Utilities;
    using UniGreenModules.UniCore.Runtime.Interfaces;

    public class GoogleSpreadsheetClient : IStringUnique
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

        #endregion

        private readonly string _sheetId;

        private List<string>                  _sheetsTitles = new List<string>();
        private MajorDimension                _dimension    = MajorDimension.Columns;
        private SheetsService                 _service;
        private Spreadsheet                   _spreadSheet;
        private Dictionary<string, SheetData> _sheetValueCache = new Dictionary<string, SheetData>(4);
        private List<SheetData>               _sheets          = new List<SheetData>();

        #region constructor

        public GoogleSpreadsheetClient(
            SheetsService service,
            string sheetId,
            MajorDimension dimension = MajorDimension.Columns)
        {
            // Create Google Sheets API service.
            _service   = service;
            _sheetId   = sheetId;
            _dimension = dimension;
        }

        #endregion

        public string Id => _sheetId;

        public IReadOnlyList<string> SheetsTitles => _sheetsTitles;

        public Spreadsheet Spreadsheet => _spreadSheet;

        public IReadOnlyList<SheetData> Sheets => GetAllSheetsData();

        public void Reload()
        {
            _sheetValueCache.Clear();
            _sheetsTitles.Clear();
            
            _spreadSheet = LoadSpreadSheet();
            _spreadSheet.Sheets.ForEach(x => _sheetsTitles.Add(x.Properties.Title));

            GetAllSheetsData();
        }

        public IReadOnlyList<SheetData> GetAllSheetsData()
        {
            _sheets.Clear();
            foreach (var sheet in _sheetsTitles) {
                var sheetData = GetData(sheet);
                _sheets.Add(sheetData);
            }

            return _sheets;
        }
        
        public SheetData GetData(string sheetId)
        {
            return _sheetValueCache.TryGetValue(sheetId, out var result) ? result : LoadData(sheetId);
        }

        public SheetData LoadData(string sheetId)
        {
            if (string.IsNullOrEmpty(sheetId))
                return null;
            // Define request parameters.
            var spreadsheetId = _sheetId;
            var targetRange   = sheetId;
            var request       = _service.Spreadsheets.Values.Get(spreadsheetId, targetRange);
            
            request = ApplyGetResourceDimmension(request);

            var response = request.Execute();
            var values   = response.Values;

            return UpdateCache(sheetId, values);
        }

        #region async methods

        public async UniTask<IReadOnlyList<SheetData>> GetAllSheetsDataAsync()
        {
            _sheets.Clear();
            foreach (var sheet in _sheetsTitles) {
                var sheetData = await GetDataAsync(sheet);
                _sheets.Add(sheetData);
            }
            return _sheets;
        }
        
        public async UniTask<SheetData> GetDataAsync(string tableRequest)
        {
            if (_sheetValueCache.TryGetValue(tableRequest, out var result)) {
                return result;
            }

            return await LoadDataAsync(tableRequest);
        }

        public async UniTask<SheetData> LoadDataAsync(string tableRequest)
        {
            if (string.IsNullOrEmpty(tableRequest))
                return null;
            // Define request parameters.
            var spreadsheetId = _sheetId;
            var targetRange   = tableRequest;
            var request       = _service.Spreadsheets.Values.Get(spreadsheetId, targetRange);

            request = ApplyGetResourceDimmension(request);

            var response = await request.ExecuteAsync();
            var values   = response.Values;

            return UpdateCache(tableRequest, values);
        }

        #endregion


        #region private methods

        private SpreadsheetsResource.ValuesResource.GetRequest ApplyGetResourceDimmension(SpreadsheetsResource.ValuesResource.GetRequest request)
        {
            switch (_dimension) {
                case MajorDimension.DimensionUnspecified:
                    request.MajorDimension = SpreadsheetsResource.ValuesResource.GetRequest.MajorDimensionEnum.DIMENSIONUNSPECIFIED;
                    break;
                case MajorDimension.Rows:
                    request.MajorDimension = SpreadsheetsResource.ValuesResource.GetRequest.MajorDimensionEnum.ROWS;
                    break;
                case MajorDimension.Columns:
                    request.MajorDimension = SpreadsheetsResource.ValuesResource.GetRequest.MajorDimensionEnum.COLUMNS;
                    break;
            }

            return request;
        }

        private SheetData UpdateCache(string sheetId, IList<IList<object>> values)
        {
            var cacheValue = new SheetData(sheetId, Id, _dimension).Update(values);
            cacheValue.Update(values);

            _sheetValueCache[sheetId] = cacheValue;
            return cacheValue;
        }

        private Spreadsheet LoadSpreadSheet()
        {
            var sheetsRequest = _service.Spreadsheets.Get(Id);
            sheetsRequest.Ranges          = new List<string>();
            sheetsRequest.IncludeGridData = false;
            var spreadSheet = sheetsRequest.Execute();
            return spreadSheet;
        }

        #endregion
    }
}