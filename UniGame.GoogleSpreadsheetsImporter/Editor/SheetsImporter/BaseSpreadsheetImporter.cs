namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System.Collections.Generic;
    using Abstract;
    using UnityEngine;

    public abstract class BaseSpreadsheetImporter : ScriptableObject, ISpreadsheetAssetsHandler
    {
        private SpreadsheetData _spreadsheetData;
        
        #region public properties
        public bool IsValidData => _spreadsheetData != null;

        #endregion

        public void Initialize(SpreadsheetData data)
        {
            _spreadsheetData = data;
        }
        
        public abstract void Load();

        public abstract List<object> Import(SpreadsheetData spreadsheetData);

        public virtual SpreadsheetData Export(SpreadsheetData data) => data;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ButtonGroup()]
        [Sirenix.OdinInspector.Button()]
        [Sirenix.OdinInspector.EnableIf("IsValidData")]
#endif
        public void Import()
        {
            Load();
            Import(_spreadsheetData);
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ButtonGroup()]
        [Sirenix.OdinInspector.Button()]
        [Sirenix.OdinInspector.EnableIf("IsValidData")]
#endif
        public void Export()
        {
            Load();
            Export(_spreadsheetData);
        }

    }
}