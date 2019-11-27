namespace UniGreenModules.UnityBuild.Editor.ClientBuild.Interfaces {
    using UnityEditor.Build.Reporting;

    public interface IUnityPostBuildCommand  : IUnityBuildCommand{

        void Execute(IUniBuilderConfiguration configuration,BuildReport buildReport = null);
        
    }
}