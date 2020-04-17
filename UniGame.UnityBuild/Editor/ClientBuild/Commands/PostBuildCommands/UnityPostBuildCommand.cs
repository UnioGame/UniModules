namespace UniGreenModules.UniGame.UnityBuild.Editor.ClientBuild.Commands.PostBuildCommands {
    using Interfaces;
    using UnityEditor.Build.Reporting;
    using UnityEngine;

    public abstract class UnityPostBuildCommand : ScriptableObject,
        IUnityBuildCommandInfo,
        IUnityPostBuildCommand
    {
        [SerializeField]
        private UnityBuildCommandInfo commandCommandInfo;

        public IUnityBuildCommandInfo Info => commandCommandInfo;
        
        public abstract void Execute(IUniBuilderConfiguration configuration,BuildReport buildReport);

        
        public int    Priority => Info.Priority;
        public bool   IsActive => Info.IsActive;
        public string Name     => Info.Name;
    }
}
