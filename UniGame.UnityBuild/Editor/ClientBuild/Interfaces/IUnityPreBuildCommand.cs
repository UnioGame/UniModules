namespace UniGreenModules.UniGame.UnityBuild.Editor.ClientBuild.Interfaces {
    public interface IUnityPreBuildCommand : IUnityBuildCommand {

        void Execute(IUniBuilderConfiguration buildParameters);
        
    }
}