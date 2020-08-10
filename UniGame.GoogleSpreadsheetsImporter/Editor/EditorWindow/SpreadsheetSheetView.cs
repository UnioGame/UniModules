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
        [Sirenix.OdinInspector.TableMatrix(SquareCells = true)]
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
            sheetValues = new string[data.RowsCount,data.ColumnsCount];
            var table = data.Table;

            for (var i = 0; i < table.Columns.Count; i++) {
                DataColumn column = table.Columns[i];
                sheetValues[i, 0] = column.ColumnName;
            }

            for (var i = 1; i < table.Columns.Count; i++) {
                for (var j = 1; j < table.Rows.Count; j++) {
                    var row = table.Rows[j];
                    var rowValue = row[i];
                    sheetValues[j,i] = rowValue == null ? string.Empty :
                        rowValue.ToString();
                }
            }
            
        }
        
    }
}