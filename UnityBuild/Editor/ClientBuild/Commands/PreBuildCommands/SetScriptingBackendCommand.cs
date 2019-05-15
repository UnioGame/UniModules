using UnityEditor;

namespace UniGreenModules.UnityBuild.Editor.ClientBuild.Commands.PreBuildCommands
{
    using Interfaces;
    using UnityEngine;

    /// <summary>
    /// update current project version
    /// </summary>
    [CreateAssetMenu(menuName = "UnityBuild/PreBuildCommands/Set Scripting Backend", fileName = "SetScriptingBackendCommand")]
    public class SetScriptingBackendCommand : UnityPreBuildCommand
    {
        [SerializeField]
        private string l2cppEnabled = "-l2cppEnabled";
        
        public override void Execute(IArgumentsProvider arguments, IBuildParameters buildParameters)
        {
            
            var scriptingBackend = arguments.Contains(l2cppEnabled) ? ScriptingImplementation.IL2CPP : ScriptingImplementation.Mono2x;
           
            switch (buildParameters.BuildTargetGroup) {
                case BuildTargetGroup.Standalone:
                case BuildTargetGroup.Android:
                    PlayerSettings.SetScriptingBackend(buildParameters.BuildTargetGroup,scriptingBackend);
                    return;
            }
            
            
        }
    }
}
