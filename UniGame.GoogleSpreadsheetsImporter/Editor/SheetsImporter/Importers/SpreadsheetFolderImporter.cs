using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter.Importers
{
    [Serializable]
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.HideLabel]
    [Sirenix.OdinInspector.BoxGroup("Attributes Source")]
#endif
    [CreateAssetMenu(menuName = "UniGame/Google/Spreadsheet/Importers/SpreadsheetFolderImporter",fileName = nameof(SpreadsheetFolderImporter))]
    public class SpreadsheetFolderImporter : SpreadsheetsSyncAssetsImporter
    {

        public List<AssetFolderImporter> folderImporters = new List<AssetFolderImporter>();
    
        public override void Load()
        {
            folderImporters.ForEach(x => x.Load());
        }

        public override void Import(SpreadsheetData spreadsheetData)
        {
            folderImporters.ForEach(x => x.Import(spreadsheetData));
        }
    }
}
