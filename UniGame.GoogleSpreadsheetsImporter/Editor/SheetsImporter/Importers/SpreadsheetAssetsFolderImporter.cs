using System;
using UnityEngine;

namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter.Importers
{
    [Serializable]
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.HideLabel]
    [Sirenix.OdinInspector.BoxGroup("Attributes Source")]
#endif
    [CreateAssetMenu(menuName = "UniGame/Google/Spreadsheet/Importers/SpreadsheetFolderImporter",fileName = nameof(SpreadsheetAssetsFolderImporter))]
    public class SpreadsheetAssetsFolderImporter : SpreadsheetImporter<AssetFolderByMonoScriptImporter>
    {
    }
}
