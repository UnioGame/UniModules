namespace UniGreenModules.UnityBuild.Editor.ClientBuild.Interfaces {
    public interface IUnityPreBuildCommand : IUnityBuildCommand {

        void Execute(IUniBuilderConfiguration buildParameters);
        
    }
}