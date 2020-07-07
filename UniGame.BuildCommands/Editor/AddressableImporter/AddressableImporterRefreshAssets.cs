namespace UniModules.UniGame.BuildCommands.Editor.AddressableImporter
{
    using System.Collections.Generic;
    using UniBuild.Editor.ClientBuild.Commands.PreBuildCommands;
    using UniBuild.Editor.ClientBuild.Interfaces;
    using UnityEngine;
    using AddressableImporter = global::AddressableImporter;

    [CreateAssetMenu(menuName = "UniGame/UniBuild/Commands/Refresh Addressable Assets", fileName = "RefreshAddressableAssets")]
    public class AddressableImporterRefreshAssets : UnityPreBuildCommand
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
