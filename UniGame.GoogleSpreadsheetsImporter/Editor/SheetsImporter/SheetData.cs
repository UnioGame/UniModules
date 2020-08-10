namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using Google.Apis.Sheets.v4.Data;
    using UniGreenModules.UniCore.Runtime.Utils;
    using UnityEngine;

    [Serializable]
    public class SheetData
    {
        private readonly MajorDimension _dimension;
        private static Func<string, string> _fieldKeyFactory = MemorizeTool.Create<string, string>(x => x.TrimStart('_').ToLower());

        private const string _spaceString = " ";
        
        private StringBuilder _stringBuilder = new StringBuilder(300);
        private DataTable _table;
        private bool _isChanged = false;

        public SheetData(string sheetId, string spreadsheetId, MajorDimension dimension)
        {
            _dimension = dimension;
            _table = new DataTable(sheetId, spreadsheetId);
        }

        #region public properties

        public bool IsChanged => _isChanged;

        public string SpreadsheetId => _table.Namespace;

        public string Id => _table.TableName;

        public DataTable Table => _table;

        public int Rows => _table.Rows.Count;

        public int Columns => _table.Columns.Count;

        public object this[int x, string y] => x == 0 ? _table.Columns[y].ColumnName : _table.Rows[x][y];

        #endregion
        
        public IList<IList<object>> CreateSource() {
            var items = new List<IList<object>>();

            foreach (DataRow row in _table.Rows) {
                items.Add(row.ItemArray.ToList());
            }
            
            return items;
        }

        public DataColumn GetColumn(string key)
        {
            var fieldKey = _fieldKeyFactory(key);
            return _table.Columns.Contains(fieldKey) ? 
                _table.Columns[fieldKey] : 
                null;
        }
        
        public void Commit()
        {
            _isChanged = false;
        }
        
        public bool UpdateValue(object value, int row, int column)
        {
            _isChanged = true;
            if (row >= Rows || column >= Columns)
                return false;
            _table.Rows[row][column] = value;
            return true;
        }
        
        public object GetValue(string key, object keyValue, string fieldName)
        {
            fieldName = _fieldKeyFactory(fieldName);
            var row = GetSliceByKeyValue(key, keyValue);
            return row[_fieldKeyFactory(fieldName)];
        }

        public SheetData Update(IList<IList<object>> source)
        {
            _isChanged = true;
            _table.Clear();
            
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
                _stringBuilder.Append(string.Join(_spaceString,row.ItemArray));
                _stringBuilder.AppendLine();
            }

            return _stringBuilder.ToString();
        }

        public DataRow GetSliceByKeyValue(string fieldName, object value)
        {
            var key = _fieldKeyFactory(fieldName);
            foreach (DataRow row in _table.Rows) {
                var rowValue = row[key];
                if (rowValue == value)
                    return row;
            }
            return null;
        }

        private void ParseSourceData(IList<IList<object>> source)
        {
            var rows    = source.Count;
            for (var i = 0; i < rows; i++) {
                var              line       = source[i];

                if (i == 0) {
                    AddHeaders(_table, line);
                }
                else {
                    AddLine(_table, line);
                }

            }
        }

        private void AddLine(DataTable table, IList<object> line)
        {
            var row = table.NewRow();
            var columns = table.Columns;
            var minLen = Mathf.Min(columns.Count, line.Count);
            if (minLen <= 0)
                return;
            for (var i = 0; i < minLen; i++) {
                row[columns[i].ColumnName] = line[i];
            }

            table.Rows.Add(row);
        }
        
        private void AddHeaders(DataTable table, IList<object> headers)
        {
            foreach (var header in headers) {
                table.Columns.Add(header==null ? string.Empty :
                    _fieldKeyFactory(header.ToString()));
            }
        }
        
    }
}