namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using Google.Apis.Sheets.v4.Data;
    using TypeConverters.Editor;
    using UniGreenModules.UniCore.Runtime.Utils;
    using UnityEditor;
    using UnityEngine;

    [Serializable]
    public class SheetData
    {
        #region static data

        private static Func<string, string> _fieldKeyFactory = MemorizeTool.Create<string, string>(x => x.TrimStart('_').ToLower());

        private const string _spaceString = " ";

        public static string FormatKey(string key) => _fieldKeyFactory(key);

        public static bool IsEquals(string key1, string key2) => FormatKey(key1) == FormatKey(key2);
        
        #endregion
        
        
        private readonly MajorDimension _dimension;
        private List<object> _headers = new List<object>();
        private          StringBuilder  _stringBuilder = new StringBuilder(300);
        private          DataTable      _table;
        private          bool           _isChanged = false;

        public SheetData(string sheetId, string spreadsheetId, MajorDimension dimension)
        {
            _dimension = dimension;
            _table     = new DataTable(sheetId, spreadsheetId);
        }

        #region public properties

        public bool IsChanged => _isChanged;

        public string SpreadsheetId => _table.Namespace;

        public string Id => _table.TableName;

        public DataTable Table => _table;

        public DataColumnCollection Columns => _table.Columns;

        public DataRowCollection Rows => _table.Rows;
        
        public int RowsCount => _table.Rows.Count;

        public int ColumnsCount => _table.Columns.Count;

        public object this[int x, string y] => x == 0 ? _table.Columns[y].ColumnName : _table.Rows[x][y];

        #endregion

        public IList<IList<object>> CreateSource()
        {
            var items = new List<IList<object>>();

            items.Add(_headers);
            
            foreach (DataRow row in _table.Rows) {
                items.Add(row.ItemArray.ToList());
            }

            return items;
        }

        public IEnumerable<object> GetColumnValues(string key)
        {
            var column = GetColumn(key);
            if (column == null)
                yield break;
            var columnName = column.ColumnName;
            foreach (DataRow row in _table.Rows) {
                yield return row[columnName];
            }
        }

        public DataColumn GetColumn(string key)
        {
            var fieldKey = _fieldKeyFactory(key);
            return _table.Columns.Contains(fieldKey) ? _table.Columns[fieldKey] : null;
        }

        public void Commit()
        {
            _isChanged = false;
        }

        public DataRow CreateRow()
        {
            _isChanged = true;
            return AddRow(_table);    
        }
        
        public bool UpdateValue(DataRow row, int column,object value)
        {
            _isChanged = true;
            if (column >= ColumnsCount)
                return false;
            row[column] = value;
            return true;
        }
        
        public bool UpdateValue(DataRow row, string fieldName,object value)
        {
            var columns      = _table.Columns;
            var columnsCount = columns.Count;

            for (var i = 0; i < columnsCount; i++) {
                var columnName = columns[i].ColumnName;
                if (!SheetData.IsEquals(columnName, fieldName)) {
                    continue;
                }

                var currentValue = row[i];
                var newValue     = value.TryConvert(typeof(string));
                
                row[i]     = newValue;
                _isChanged = true;
                break;
            }

            return true;
        }

        public object GetValue(string key, object keyValue, string fieldName)
        {
            fieldName = _fieldKeyFactory(fieldName);
            var row = GetRow(key, keyValue);
            return row?[fieldName];
        }

        public SheetData Update(IList<IList<object>> source)
        {
            _isChanged = true;
            _table.Clear();
            _headers.Clear();

            ParseSourceData(source);

            return this;
        }

        public bool HasData(string key)
        {
            return _table.Columns.Contains(_fieldKeyFactory(key));
        }

        public bool AddValue(string key, object value)
        {
            var columnKey = _fieldKeyFactory(key);
            if (!_table.Columns.Contains(columnKey))
                return false;
            var row = _table.NewRow();
            foreach (DataColumn column in _table.Columns) {
                row[column.ColumnName] = string.Empty;
            }

            row[columnKey] = value;
            return true;
        }

        public override string ToString()
        {
            _stringBuilder.Clear();
            var columns = _table.Columns;
            foreach (DataColumn column in columns) {
                _stringBuilder.Append(column.ColumnName);
                _stringBuilder.Append(_spaceString);
            }

            _stringBuilder.AppendLine();

            foreach (DataRow row in _table.Rows) {
                _stringBuilder.Append(string.Join(_spaceString, row.ItemArray));
                _stringBuilder.AppendLine();
            }

            return _stringBuilder.ToString();
        }

        public DataRow GetRow(string fieldName, object value)
        {
            var key = _fieldKeyFactory(fieldName);
            for (var i = 0; i < _table.Rows.Count; i++) {
                var row      = _table.Rows[i];
                var     rowValue = row[key];
                if (Equals(rowValue, value))
                    return row;
            }

            return null;
        }

        private void ParseSourceData(IList<IList<object>> source)
        {
            var rows = source.Count;
            for (var i = 0; i < rows; i++) {
                var line = source[i];

                if (i == 0) {
                    AddHeaders(_table, line);
                }
                else {
                    AddRow(_table, line);
                }
            }
        }

        private DataRow AddRow(DataTable table, IList<object> line = null)
        {
            var row     = table.NewRow();
            var columns = table.Columns;
            var lineLen = line?.Count ?? 0;
            if (columns.Count <= 0)
                return row;
            for (var i = 0; i < columns.Count; i++) {
                row[columns[i].ColumnName] = i < lineLen ? line[i] : string.Empty;
            }

            table.Rows.Add(row);
            return row;
        }

        private void AddHeaders(DataTable table, IList<object> headers)
        {
            var counter = 0;
            var columns = table.Columns;
            foreach (var header in headers) {
                counter++;
                var title = header == null ? string.Empty : header.ToString();
                _headers.Add(title);
                title = _fieldKeyFactory(title);
                title = columns.Contains(title) ? 
                    Guid.NewGuid().ToString().
                        Substring(0,10) : 
                    title;
                columns.Add(title);
            }
        }
    }
}