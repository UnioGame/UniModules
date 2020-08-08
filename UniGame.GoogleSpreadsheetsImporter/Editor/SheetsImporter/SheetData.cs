namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UniGreenModules.UniCore.Runtime.Utils;

    [Serializable]
    public class SheetData
    {
        private static Func<string, string> _fieldKeyFactory = MemorizeTool.Create<string, string>(x => x.TrimStart('_').ToLower());
        private StringBuilder _stringBuilder = new StringBuilder(300);
        
        private string                   _id;
        private string                   _spreadsheetId;
        private MajorDimension           _dimension;
        private int _rows;
        private int _columns;
        private Dictionary<string,SheetLineData>      _lines        = new  Dictionary<string,SheetLineData>(4);
        private IList<IList<object>>     _sourceData   = new List<IList<object>>();
        private IList<IList<SheetValue>> _sourceValues = new List<IList<SheetValue>>();

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

        public IReadOnlyDictionary<string,SheetLineData> Values => _lines;

        public IList<IList<object>> Source => _sourceData;

        public int Rows => _rows;

        public int Columns => _columns;

        public SheetValue this[int x, int y] {
            get {
                if (x >= _sourceValues.Count)
                    return null;
                var values =  _sourceValues[x];
                return y >= values.Count ? null : values[y];
            }
        }

        #endregion

        public void UpdateValue(object value, int row, int column)
        {
            _sourceValues[row][column].value = value;
            _sourceData[row][column]         = value;
        }

        public SheetData Update(IList<IList<object>> source)
        {
            _sourceData = source;
            _lines.Clear();
            _sourceValues.Clear();

            ParseSourceData(source);
            
            return this;
        }

        public bool HasData(string key)
        {
            return _lines.ContainsKey(_fieldKeyFactory(key));
        }

        public SheetLineData GetData(string key)
        {
            _lines.TryGetValue(_fieldKeyFactory(key), out var line);
            return line;
        }

        public SheetValue GetValue(string key, object keyValue, string fieldName)
        {
            fieldName = _fieldKeyFactory(fieldName);
            foreach (var value in GetSliceByKeyValue(_fieldKeyFactory(key),keyValue)) {
                if (value.fieldName == fieldName)
                    return value;
            }

            return null;
        }

        public IEnumerable<SheetValue> GetSliceByKeyValue(string fieldName, object value)
        {
            var line = GetData(fieldName);

            var sheetValue = line?.data.
                FirstOrDefault(x => x.value == value);
            if (sheetValue == null)
                yield break;

            for (var i = 0; i < _sourceValues.Count; i++) {
                yield return _sourceValues[i][sheetValue.column];
            }
        }

        private void ParseSourceData(IList<IList<object>> source)
        {
            _rows    = source.Count;
            for (var i = 0; i < _rows; i++) {
                var              line       = source[i];
                List<SheetValue> sourceLine = null;
                var              key        = string.Empty;
                var              lineData   = new SheetLineData();

                _columns = line.Count;
                
                for (var j = 0; j < _columns; j++) {
                    var item = line[j];

                    if (j == 0) {
                        sourceLine = new List<SheetValue>();
                        _sourceValues.Add(sourceLine);
                        key            = item.ToString();
                        lineData.id    = key;
                        lineData.index = i;
                        _lines[_fieldKeyFactory(key)] = lineData;
                    }

                    if (string.IsNullOrEmpty(key))
                        continue;

                    var value = new SheetValue() {
                        row       = i,
                        column    = j,
                        value     = item,
                        fieldName = _fieldKeyFactory(key),
                        sheetName = _id
                    };

                    sourceLine.Add(value);

                    if (j > 0) {
                        lineData.data.Add(value);
                    }
                }

            }
        }

        public override string ToString()
        {
            for (int i = 0; i < _sourceValues.Count; i++) {
                var line = _sourceValues[i];
                for (int j = 0; j < UPPER; j++) {
                    
                }
            }
        }
    }
}