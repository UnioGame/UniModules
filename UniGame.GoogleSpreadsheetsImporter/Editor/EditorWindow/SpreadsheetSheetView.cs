namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.EditorWindow
{
    using System.Linq;
    using SheetsImporter;
    using UnityEngine;

    public class SpreadsheetSheetView : ScriptableObject
    {
        public string sheetName;

        public string spreadSheetId;
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.TableMatrix(SquareCells = true)]
#endif
        public string[,] sheetValues;
        
        public void Initialize(SheetData data)
        {
            spreadSheetId = data.SpreadsheetId;
            sheetName = data.Id;
            UpdateView(data);
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
            
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    if (i == 0) {
                        sheetValues[i, j] = lines[i].id;
                        continue;
                    }

                    var values = lines[i].data;
                    sheetValues[i, j] = j >= values.Count ? string.Empty : values[j]?.ToString();
                }
            }
            
        }
        
    }
}