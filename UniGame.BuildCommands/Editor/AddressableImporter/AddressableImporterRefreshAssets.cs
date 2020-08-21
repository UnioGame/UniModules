namespace UniModules.UniGame.BuildCommands.Editor.AddressableImporter
{
    using System;
    using System.Collections.Generic;
    using UniBuild.Editor.ClientBuild.Commands.PreBuildCommands;
    using UniBuild.Editor.ClientBuild.Interfaces;
    using AddressableImporter = global::AddressableImporter;

    [Serializable]
     public class AddressableImporterRefreshAssets : UnitySerializablePreBuildCommand
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FolderPath]
#endif
        public List<string> reimportPaths = new List<string>() {
            "Assets"
        };
        
        public override void Execute(IUniBuilderConfiguration buildParameters)
        {
            RefreshAddressables();
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void RefreshAddressables()
        {
            AddressableImporter.FolderImporter.ReimportFolders(reimportPaths);
        }
    }
}
