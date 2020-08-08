namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.EditorWindow
{
    using System.Linq;
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
            if (data.Values.Count == 0)
                return;

            sheetValues = new string[data.Rows,data.Columns];

            for (var i = 0; i < data.Rows; i++) {
                for (var j = 0; j < data.Columns; j++) {
                    var valueItem = data[i,j];//new SheetCellView();
                    sheetValues[i, j] = valueItem == null ? string.Empty :
                        j == 0 ? valueItem.fieldName :
                        valueItem.value.ToString();
                }
            }
            
        }
        
    }
}