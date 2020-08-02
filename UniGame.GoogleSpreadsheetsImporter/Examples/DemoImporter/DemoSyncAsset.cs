using UnityEngine;

namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Examples.DemoImporter
{
    using GoogleSpreadsheets.Runtime.Attributes;

    [SpreadsheetTarget(keyField:nameof(id),sheetName:nameof(DemoSyncAsset))]
    [CreateAssetMenu(menuName = "UniGame/Google/Spreadsheet/Examples/DemoSyncAsset",fileName = nameof(DemoSyncAsset))]
    public class DemoSyncAsset : ScriptableObject
    {

        public string id;

        public string stringValue;

        public int intValue;
        
        public float floatValue;

        public ScriptableObject assetValue;

    }
}
