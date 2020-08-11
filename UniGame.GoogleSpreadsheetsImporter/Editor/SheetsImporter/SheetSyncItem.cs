namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using Object = UnityEngine.Object;

    [Serializable]
    public class SheetSyncItem
    {
        public string sheetName;
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineEditor]
#endif
        public Object asset;
    }
}