namespace UniGreenModules.UnityBuild.Editor.ClientBuild.Commands {
    using Interfaces;
    using UnityEngine;

    public abstract class UnityPreBuildCommand : ScriptableObject,IUnityPreBuildCommand {

        [SerializeField]
        private UnityBuildCommandInfo commandCommandInfo;

        public IUnityBuildCommandInfo Info => commandCommandInfo;
        
        public abstract void Execute(IArgumentsProvider arguments, IBuildParameters buildParameters);

    }
}
