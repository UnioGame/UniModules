namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.EditorWindow
{
    using System;
    using UnityEngine;

    [Serializable]
    public class SheetCellView : ScriptableObject
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.HideLabel]
        public string value; 
#endif
        [HideInInspector]
        public int row;
        [HideInInspector]
        public int column;
        [HideInInspector]
        public bool isHeader;
    }
}