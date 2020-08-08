namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter.Importers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.EditorTools.Editor.AssetOperations;
    using Extensions;
    using GoogleSpreadsheets.Runtime.Attributes;
    using UnityEngine;
    using Object = UnityEngine.Object;

    [Serializable]
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.HideLabel]
    [Sirenix.OdinInspector.BoxGroup("Attributes Source")]
#endif
    [CreateAssetMenu(menuName = "UniGame/Google/Spreadsheet/Importers/AssetsWithAttributesImporter",fileName = nameof(AssetsWithAttributesImporter))]
    public class SpreadsheetAssetImporter :  BaseSpreadsheetImporter 
    {
        
        /// <summary>
        /// list of assets linked by attributes
        /// </summary>
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.HideLabel]
        [Sirenix.OdinInspector.InlineProperty]
#endif
        public SheetSyncItem asset = new SheetSyncItem();
        
        public override void Load()
        {

        }

        public override List<object> Import(SpreadsheetData spreadsheetData)
        {
            var item = asset?.ApplySpreadsheetData(spreadsheetData);
            return new List<object>(){item?.asset};
        }
    }
}