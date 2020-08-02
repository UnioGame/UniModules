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
                new Color(0.1f,0.8f,0.2f) :
                new Color(0.0f,0.0f,0.5f));
            
            EditorGUILayout.LabelField(cellView.value);
            
            return cellView;
        }
        
        private void UpdateView(SheetData data)
        {
            var direction  = data.Dimension;
            var lines      = data.Values;
            if (lines.Count == 0)
                return;
            
            var isVertical = direction == MajorDimension.Columns;
            var maxLength  = lines.Max(x => x.data.Count)+1;
            var width      = isVertical ? lines.Count : maxLength;
            var height     = isVertical ? maxLength : lines.Count;

            sheetValues = new string[width,height];

            var tempHeight = height;
            height = isVertical ? tempHeight : width;
            width  = isVertical ? width : tempHeight;
            
            for (var i = 0; i < width; i++) {
                for (var j = 0; j < height; j++) {
                    var valueItem = string.Empty;//new SheetCellView();
                    if (j == 0) {
                        valueItem = lines[i].id;
                        sheetValues[i, j] = valueItem;
                        continue;
                    }

                    var values = lines[i].data;
                    valueItem = j >= values.Count ? 
                        string.Empty : 
                        values[j]?.ToString();
                    sheetValues[i, j] = valueItem;
                }
            }
            
        }
        
    }
}