using System;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace UniGreenModules.UniGame.UnityBuild.Editor.ClientBuild
{
    using BuildConfiguration;
    using Interfaces;
    using UniModules.UniGame.UnityBuild.Editor.ClientBuild.BuildConfiguration;

    public static class UnityBuildTool
    {

        public const string BuildFolder = "Build";

        private static UnityPlayerBuilder builder = new UnityPlayerBuilder();
    
        /// <summary>
        /// Console build call. Close editor after end of build process
        /// </summary>
        public static void BuildUnityPlayer()
        {
            var configuration = new UniBuilderConsoleConfiguration(Environment.GetCommandLineArgs());
        
            var report = BuildPlayer(configuration);

            EditorApplication.Exit(report.summary.result == BuildResult.Succeeded ? 0 : 1);
        }

        public static BuildReport BuildPlayer(IUniBuilderConfiguration configuration)
        {
            var report = builder.Build(configuration);
            return report;
        }

        public static BuildReport BuildPlayer(IUniBuilderConfiguration configuration, IUniBuildCommandsMap commandsMap)
        {
            var report = builder.Build(configuration,commandsMap);
            return report;
        }
        
    }
}
