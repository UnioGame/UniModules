namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;
    using Abstract;
    using UniRx;
    using UnityEngine;

    public abstract class BaseSpreadsheetImporter : ScriptableObject, ISpreadsheetAssetsHandler
    {
        private ISubject<ISpreadsheetAssetsHandler> _importCommand;
        private ISubject<ISpreadsheetAssetsHandler> _exportCommand;
        
        #region public properties
        public bool IsValidData => _importCommand != null && _exportCommand !=null;

        public IObservable<ISpreadsheetAssetsHandler> ImportCommand => _importCommand;

        public IObservable<ISpreadsheetAssetsHandler> ExportCommand => _exportCommand;
        
        #endregion

        public void Initialize()
        {
            _importCommand = new Subject<ISpreadsheetAssetsHandler>();
            _exportCommand = new Subject<ISpreadsheetAssetsHandler>();
        }

        public abstract IEnumerable<object> Load();

        public abstract List<object> Import(SpreadsheetData spreadsheetData);

        public virtual SpreadsheetData Export(SpreadsheetData data) => data;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ButtonGroup()]
        [Sirenix.OdinInspector.Button()]
        [Sirenix.OdinInspector.ShowIf("IsValidData")]
#endif
        public void Import()
        {
            _importCommand?.OnNext(this);
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ButtonGroup()]
        [Sirenix.OdinInspector.Button()]
        [Sirenix.OdinInspector.ShowIf("IsValidData")]
#endif
        public void Export()
        {
            _exportCommand?.OnNext(this);
        }

    }
}