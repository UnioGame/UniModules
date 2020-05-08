namespace UniGreenModules.UniGame.UnityBuild.Editor.ClientBuild.Commands.PreBuildCommands
{
    using Interfaces;
    using UnityEngine;

    public abstract class UnityBuildCommand : ScriptableObject,IUnityBuildCommandValidator
    {
        [SerializeField]
        public UnityBuildCommandInfo commandCommandInfo;
        
        public IUnityBuildCommandInfo Info => commandCommandInfo;

        public bool IsActive => Info.IsActive;
        public string Name => Info.Name;
        
        public bool Validate(IUniBuilderConfiguration config) => true;
    }
}