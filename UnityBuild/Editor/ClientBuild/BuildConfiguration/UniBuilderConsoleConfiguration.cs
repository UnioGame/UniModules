namespace UniGreenModules.UnityBuild.Editor.ClientBuild
{
    using System;
    using System.Linq;
    using Interfaces;
    using Parsers;
    using UnityEditor;
    using UnityEngine;

    public class UniBuilderConsoleConfiguration : IUniBuilderConfiguration
    {
        private EnumArgumentParser<BuildTarget> buildTargetParser;
        private EnumArgumentParser<BuildTargetGroup> buildTargetGroupParser;
        
        private ArgumentsProvider argumentsProvider;
        private BuildParameters buildParameters;
        
        public UniBuilderConsoleConfiguration(string[] commandLineArgs)
        {
            buildTargetParser = new EnumArgumentParser<BuildTarget>();
            buildTargetGroupParser = new EnumArgumentParser<BuildTargetGroup>();
            
            argumentsProvider = new ArgumentsProvider(commandLineArgs);

            var buildTarget      = GetBuildTarget(argumentsProvider);
            var buildTargetGroup = GetBuildTargetGroup(argumentsProvider);

            buildParameters = new BuildParameters(buildTarget,buildTargetGroup, argumentsProvider);

            Debug.Log(argumentsProvider);
            
        }

        public IArgumentsProvider Arguments => argumentsProvider;

        public IBuildParameters BuildParameters => buildParameters;


#region private methods     
        
        public BuildTarget GetBuildTarget(IArgumentsProvider arguments)
        {
            var targets = buildTargetParser.Parse(arguments);
            return targets.Count > 0 ?
                targets.FirstOrDefault() :
                EditorUserBuildSettings.activeBuildTarget;
        }

        public BuildTargetGroup GetBuildTargetGroup(IArgumentsProvider arguments)
        {
            var groups = buildTargetGroupParser.Parse(arguments);
            return groups.Count > 0 ? groups.First() :
                EditorUserBuildSettings.selectedBuildTargetGroup;
        }
        
#endregion
        
    }
}
