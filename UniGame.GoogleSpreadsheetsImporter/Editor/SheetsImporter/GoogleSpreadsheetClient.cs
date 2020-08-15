namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using Google.Apis.Sheets.v4;
    using Google.Apis.Sheets.v4.Data;
    using Sirenix.Utilities;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniCore.Runtime.Utils;
    using UnityEngine;

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

        private readonly string _spreadsheetId;

        private List<string>                  _sheetsTitles = new List<string>();
        private MajorDimension                _dimension    = MajorDimension.COLUMNS;
        private SheetsService                 _service;
        private Spreadsheet                   _spreadSheet;
        private Dictionary<string, SheetData> _sheetValueCache = new Dictionary<string, SheetData>(4);
        private List<SheetData>               _sheets          = new List<SheetData>();

        #region constructor

        public GoogleSpreadsheetClient(
            SheetsService service,
            string spreadsheetId,
            MajorDimension dimension = MajorDimension.ROWS)
        {
            // Create Google Sheets API service.
            _service   = service;
            _spreadsheetId   = spreadsheetId;
            _dimension = dimension;
        }

        #endregion

        public string Id => _spreadsheetId;

        public IReadOnlyList<string> SheetsTitles => _sheetsTitles;

        public Spreadsheet Spreadsheet => _spreadSheet;

        public IReadOnlyList<SheetData> Sheets => GetAllSheetsData();

        
        
        public bool HasSheet(string id)
        {
            return Sheets.Any(x => string.Equals(x.Id, id, StringComparison.OrdinalIgnoreCase));
        }
        
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
                var sheetData = GetSheetData(sheet);
                _sheets.Add(sheetData);
            }

            return _sheets;
        }
        
        public SheetData GetSheetData(string sheetId)
        {
            return _sheetValueCache.TryGetValue(sheetId, out var result) ? result : LoadData(sheetId);
        }

        public SheetData UpdateData(SheetData data)
        {
            var request = CreateUpdateRequest(data);

            try {
                var response = request.Execute();
                data.Commit();
            }
            catch (Exception e) {
                Debug.LogError(e);
            }
            
            return data;
        }

        public SheetData LoadData(string sheetId)
        {
            var request       = CreateGetRequest(sheetId);
            var response = request.Execute();
            var values   = response.Values;

            return UpdateCache(sheetId, values);
        }

        #region async methods

        public async UniTask<SheetData> UpdateDataAsync(SheetData data)
        {
            
            var request = CreateUpdateRequest(data);
            try {
                var response = await request.ExecuteAsync();
                data.Commit();
            }
            catch (Exception e) {
                Debug.LogError(e);
            }

            return data;
        }
        
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
            var request = CreateGetRequest(tableRequest);
            var response = await request.ExecuteAsync();
            var values   = response.Values;

            return UpdateCache(tableRequest, values);
        }
        
        #endregion

        public SpreadsheetsResource.ValuesResource.GetRequest CreateGetRequest(string sheetId)
        {
            if (string.IsNullOrEmpty(sheetId))
                return null;
            // Define request parameters.
            var spreadsheetId = _spreadsheetId;
            var targetRange   = sheetId;
            var request       = _service.Spreadsheets.Values.Get(spreadsheetId, targetRange);
            request.MajorDimension = GetResourceDimmension();

            return request;
        }
        
        public SpreadsheetsResource.ValuesResource.UpdateRequest CreateUpdateRequest(SheetData data)
        {
            var sheetId = data.Id;

            var a1Range      = $"{sheetId}";
            var sourceValues = data.CreateSource();
            var valueRange = new ValueRange() {
                Values         = sourceValues,
                Range          = a1Range,
                MajorDimension = _dimension.ToStringFromCache()
            };

            var request = _service.Spreadsheets.Values.Update(valueRange, Id, a1Range);
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.
                UpdateRequest.ValueInputOptionEnum.USERENTERED;

            return request;
        }

        #region private methods

        private SpreadsheetsResource.ValuesResource.GetRequest.MajorDimensionEnum GetResourceDimmension()
        {
            switch (_dimension) {
                case MajorDimension.DimensionUnspecified:
                    return SpreadsheetsResource.ValuesResource.GetRequest.MajorDimensionEnum.DIMENSIONUNSPECIFIED;
                case MajorDimension.ROWS:
                    return SpreadsheetsResource.ValuesResource.GetRequest.MajorDimensionEnum.ROWS;
                case MajorDimension.COLUMNS:
                    return SpreadsheetsResource.ValuesResource.GetRequest.MajorDimensionEnum.COLUMNS;
            }

            return SpreadsheetsResource.ValuesResource.GetRequest.MajorDimensionEnum.ROWS;
        }

        private SheetData UpdateCache(string sheetId, IList<IList<object>> values)
        {
            var cacheValue = new SheetData(sheetId, Id, _dimension);
            cacheValue.Update(values);
            cacheValue.Commit();
            
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