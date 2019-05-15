namespace UniGreenModules.UnityBuild.Editor.ClientBuild.Commands.PreBuildCommands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using UnityEditor;

    /// <summary>
    /// https://docs.unity3d.com/ScriptReference/BuildOptions.html
    /// any build parameter and be used by template "-[BuildOptions Item]"
    /// </summary>
    public class BuildOptionsCommand : UnityPreBuildCommand
    {

        private static List<string> buildOptionsNames = Enum.GetNames(typeof(BuildOptions)).
            Select(x => x.Insert(0, "-").ToLower()).
            ToList();
        
        private static List<BuildOptions> buildOptionsValues = Enum.GetValues(typeof(BuildOptions)).
            OfType<BuildOptions>().
            ToList();
        
        public override void Execute(IArgumentsProvider arguments, IBuildParameters buildParameters)
        {
            var buildOptions = GetBuildOptions(arguments);
            buildParameters.SetBuildOptions(buildOptions,false);
        }
        
        public BuildOptions GetBuildOptions(IArgumentsProvider arguments)
        {
            var options = BuildOptions.None;

            for (var i = 0; i < buildOptionsValues.Count; i++) {
                var optionName = buildOptionsNames[i];
                if (arguments.Contains(optionName))
                {
                    options |= buildOptionsValues[i];
                }
            }

            return options;
        }
    }
}
