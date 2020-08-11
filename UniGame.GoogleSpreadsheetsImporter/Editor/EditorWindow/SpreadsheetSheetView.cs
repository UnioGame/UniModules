namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.EditorWindow
{
    using System.Data;
    using SheetsImporter;
    using Sirenix.Utilities;
    using UnityEditor;
    using UnityEngine;

    public class SpreadsheetSheetView : ScriptableObject
    {
        public string sheetName;

        public string spreadSheetId;
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
        [Sirenix.OdinInspector.TableMatrix(ResizableColumns = true,IsReadOnly = true)]
#endif
        public string[,] sheetValues;
        
        public void Initialize(SheetData data)
        {
            spreadSheetId = data.SpreadsheetId;
            sheetName = data.Id;
            UpdateView(data);
        }

        private static SheetCellView DrawCell(Rect rect, SheetCellView cellView)
        {
            EditorGUI.DrawRect(rect.Padding(1),cellView.isHeader ?
                new Color(0.4f,0.4f,0.4f) :
                new Color(0.0f,0.0f,0.0f));
            
            EditorGUILayout.LabelField(cellView.value);
            
            return cellView;
        }
        
        private void UpdateView(SheetData data)
        {
            sheetValues = new string[data.ColumnsCount,data.RowsCount+1];
            var table = data.Table;
            var columns = table.Columns;
            var rows = table.Rows;

            for (var i = 0; i < data.ColumnsCount; i++) {
                var column = columns[i];
                sheetValues[i, 0] = column.ColumnName;
            }

            for (var i = 0; i < data.ColumnsCount; i++) {
                for (var j = 0; j < data.RowsCount; j++) {
                    var row = rows[j];
                    var rowValue = row[i];
                    sheetValues[i,j+1] = rowValue == null ? string.Empty :
                        rowValue.ToString();
                }
            }
            
        }
        
    }
}