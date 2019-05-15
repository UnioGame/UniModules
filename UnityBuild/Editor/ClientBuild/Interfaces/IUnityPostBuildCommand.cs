namespace UniGreenModules.UnityBuild.Editor.ClientBuild.Interfaces {
    using UnityEditor.Build.Reporting;

    public interface IUnityPostBuildCommand  : IUnityBuildCommand{

        void Execute(IArgumentsProvider arguments, IBuildParameters buildParameters,BuildReport buildReport = null);
        
    }
}