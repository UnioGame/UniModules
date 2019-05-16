namespace UniGreenModules.UnityBuild.Editor.ClientBuild.Commands {
    using Interfaces;
    using UnityEditor.Build.Reporting;
    using UnityEngine;

    public abstract class UnityPostBuildCommand : ScriptableObject, IUnityPostBuildCommand
    {
        [SerializeField]
        private UnityBuildCommandInfo commandCommandInfo;

        public IUnityBuildCommandInfo Info => commandCommandInfo;
        
        public abstract void Execute(IUniBuilderConfiguration configuration,BuildReport buildReport);

    }
}
