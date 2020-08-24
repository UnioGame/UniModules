namespace UniModules.UniGame.BuildCommands.Editor.Addressables
{
    using System;
    using AddressableTools.Editor.AddressableSpriteAtlasManager;
    using UniBuild.Editor.ClientBuild.Commands.PreBuildCommands;
    using UniBuild.Editor.ClientBuild.Interfaces;
    using UnityEngine;

    [Serializable]
    public class RebuildAddressablesAtlasesCommand : UnitySerializablePreBuildCommand
    {
        public override void Execute(IUniBuilderConfiguration buildParameters)
        {
            AddressableSpriteAtlasesEditorHandler.Reimport();
        }
    }
}
