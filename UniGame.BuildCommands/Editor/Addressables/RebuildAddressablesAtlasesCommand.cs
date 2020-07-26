namespace UniModules.UniGame.BuildCommands.Editor.Addressables
{
    using AddressableTools.Editor.AddressableSpriteAtlasManager;
    using UniBuild.Editor.ClientBuild.Commands.PreBuildCommands;
    using UniBuild.Editor.ClientBuild.Interfaces;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniGame/UniBuild/Commands/Rebuild AddressablesAtlases", fileName = nameof(RebuildAddressablesAtlasesCommand))]
    public class RebuildAddressablesAtlasesCommand : UnityPreBuildCommand
    {
        public override void Execute(IUniBuilderConfiguration buildParameters)
        {
            AddressableSpriteAtlasesEditorHandler.Reimport();
        }
    }
}
