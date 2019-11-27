using System;
using System.Collections.Generic;
using System.Linq;
using UniGreenModules.UniCore.EditorTools.AssetOperations;
using UniGreenModules.UnityBuild.Editor.ClientBuild;
using UniGreenModules.UnityBuild.Editor.ClientBuild.Commands;
using UniGreenModules.UnityBuild.Editor.ClientBuild.Interfaces;
using UniModule.UnityTools.EditorTools;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using Object = UnityEngine.Object;

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

}
