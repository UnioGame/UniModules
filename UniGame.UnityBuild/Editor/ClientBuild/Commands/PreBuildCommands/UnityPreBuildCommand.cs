namespace UniGreenModules.UniGame.UnityBuild.Editor.ClientBuild.Commands.PreBuildCommands {
    using Interfaces;
    using UnityEngine;

    public abstract class UnityPreBuildCommand : ScriptableObject,
        IUnityPreBuildCommand,
        IUnityBuildCommandInfo
    {

        [SerializeField]
        public UnityBuildCommandInfo commandCommandInfo;

        public IUnityBuildCommandInfo Info => commandCommandInfo;

        public bool Validate(IUniBuilderConfiguration config) => true;

        public abstract void Execute(IUniBuilderConfiguration buildParameters);

        public int Priority => Info.Priority;
        public bool IsActive => Info.IsActive;
        public string Name => Info.Name;
    }
}
