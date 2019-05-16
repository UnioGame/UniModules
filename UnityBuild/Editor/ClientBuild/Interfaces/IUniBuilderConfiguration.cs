namespace UniGreenModules.UnityBuild.Editor.ClientBuild
{
    using Interfaces;
    using UnityEditor;

    public interface IUniBuilderConfiguration
    {
        
        IArgumentsProvider Arguments { get; }
    
        IBuildParameters BuildParameters { get; }
        
    }
}