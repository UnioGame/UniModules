using UnityEngine;

namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Examples.DemoImporter
{
    using System;
    using GoogleSpreadsheets.Runtime.Attributes;
    using UniGreenModules.UniGame.Core.Runtime.SerializableType;
    using UniGreenModules.UniGame.Core.Runtime.SerializableType.Attributes;

    [SpreadsheetTarget(keyField:nameof(id),sheetName:nameof(DemoSyncAsset))]
    [CreateAssetMenu(menuName = "UniGame/Google/Spreadsheet/Examples/DemoSyncAsset",fileName = nameof(DemoSyncAsset))]
    public class DemoSyncAsset : ScriptableObject
    {
        [STypeFilter(typeof(ScriptableObject))]
        public SType type;
        
        public string id;

        public string stringValue;

        public int intValue;
        
        public float floatValue;

        public ScriptableObject assetValue;

    }
}
