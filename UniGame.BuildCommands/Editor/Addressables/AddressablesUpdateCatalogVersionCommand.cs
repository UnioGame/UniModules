namespace UniModules.UniGame.BuildCommands.Editor.Addressables
{
    using System;
    using UniBuild.Editor.ClientBuild.Commands.PreBuildCommands;
    using UniBuild.Editor.ClientBuild.Interfaces;
    using UnityEngine;

    [Serializable]
    public class AddressablesUpdateCatalogVersionCommand : UnitySerializablePreBuildCommand
    {
        public bool   useAppVersion = true;
        public bool   useBuildNumber = false;
        public string manualVersion = Application.version;
        
        public override void Execute(IUniBuilderConfiguration buildParameters)
        {
            var version = Application.version;
        }
    }
}
