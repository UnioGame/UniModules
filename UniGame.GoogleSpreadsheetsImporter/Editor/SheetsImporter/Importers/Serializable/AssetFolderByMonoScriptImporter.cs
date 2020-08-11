namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter.Importers
{
    using System;
    using Serializable;
    using UnityEditor;

    [Serializable]
    public class AssetFolderByMonoScriptImporter : FolderAssetsImporter
    {

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.VerticalGroup("Filter")]
        [Sirenix.OdinInspector.LabelWidth(50)]
        [Sirenix.OdinInspector.LabelText("Type")]
        [Sirenix.OdinInspector.Required]
#endif
        public MonoScript assetTypeScript;


        protected override Type GetFilteredType() => assetTypeScript?.GetClass();
    }
}