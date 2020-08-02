namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SheetData
    {
        private          string               _id;
        private          string               _spreadsheetId;
        private readonly MajorDimension       _dimension;
        private          List<SheetLineData>  _lines  = new List<SheetLineData>();
        private          List<SheetSliceData> _slices = new List<SheetSliceData>();
        private          IList<IList<object>> _sourceData;

        public SheetData(string sheetId, string spreadsheetId, MajorDimension dimension)
        {
            _id            = sheetId;
            _spreadsheetId = spreadsheetId;
            _dimension     = dimension;
        }

        #region public properties

        public string SpreadsheetId => _spreadsheetId;

        public string Id => _id;

        public MajorDimension Dimension => _dimension;

        public List<SheetLineData> Values => _lines;

        public IList<IList<object>> Source => _sourceData;

        #endregion

        public SheetData Update(IList<IList<object>> source)
        {
            _sourceData = source;
            _lines.Clear();
            _slices.Clear();

            ParseSourceData(source);

            return this;
        }

        public SheetSliceData GetSliceByKeyValue(string fieldName, string value)
        {
            var result = _slices.FirstOrDefault(x => x.keyId == fieldName && x.keyValue == value);
            if (result != null) return result;

            result = new SheetSliceData() {
                sheetId  = _id,
                keyId    = fieldName,
                keyValue = value
            };

            var line = _lines.FirstOrDefault(x =>
                string.Equals(x.id, fieldName, StringComparison.OrdinalIgnoreCase));
            if (line == null)
                return result;

            var index = -1;
            var data  = line.data;
            for (var i = 0; i < data.Count; i++) {
                var dataValue = data[i];
                if (!string.Equals(dataValue.ToString(), value, StringComparison.OrdinalIgnoreCase)) {
                    continue;
                }

                index        = i;
                result.index = index;
                break;
            }

            if (index < 0) return result;

            foreach (var lineData in _lines) {
                if (lineData == line)
                    continue;
                var items       = lineData.data;
                var isValidData = lineData.data.Count > index;
                result.data.Add(new SheetValue() {
                    index     = isValidData ? index : -1,
                    value     = isValidData ? items[index] : string.Empty,
                    fieldName = lineData.id,
                    sheetName = _id
                });
            }

            _slices.Add(result);

            return result;
        }

        private void ParseSourceData(IList<IList<object>> source)
        {
            foreach (var line in source) {
                var index = -1;
                var key   = line.FirstOrDefault()?.ToString() ?? string.Empty;

                if (string.IsNullOrEmpty(key))
                    continue;

                var lineData = new SheetLineData() {
                    id         = key,
                    sourceData = line
                };

                foreach (var item in line.Skip(1)) {
                    lineData.data.Add(item);
                }

                _lines.Add(lineData);
            }
        }
    }
}