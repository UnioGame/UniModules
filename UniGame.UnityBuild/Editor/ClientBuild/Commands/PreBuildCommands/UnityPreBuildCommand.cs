namespace UniGreenModules.UniGame.UnityBuild.Editor.ClientBuild.Commands.PreBuildCommands {
    using Interfaces;
    using UnityEngine;

    public abstract class UnityPreBuildCommand : ScriptableObject,IUnityPreBuildCommand {

        [SerializeField]
        private UnityBuildCommandInfo commandCommandInfo;

        public IUnityBuildCommandInfo Info => commandCommandInfo;
        
        public abstract void Execute(IUniBuilderConfiguration buildParameters);

    }
}
