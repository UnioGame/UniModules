namespace UniGreenModules.UniGame.UnityBuild.Editor.ClientBuild.Interfaces 
{
    public interface IUnityBuildCommandInfo {
        
        int Order { get; }

        bool IsActive { get; }

        string Name { get; }

    }
}