using UnityEngine;
using UnityEditor;
using System;
using UniModule.UnityTools.EditorTools;

#if UNITY_CLOUD_BUILD

//https://docs.unity3d.com/Manual/UnityCloudBuildManifestAsScriptableObject.html
//https://docs.unity3d.com/Manual/UnityCloudBuildManifest.html
public class CloudBuildHelper : MonoBehaviour
{
    private static int buildNumber;
    private static string bundleId;
    private static string projectId;
    private static BuildTarget buildTarget;
    
    public static void PreExport(UnityEngine.CloudBuild.BuildManifestObject manifest) {
        buildNumber          = manifest.GetValue<int>("buildNumber");
        bundleId             = manifest.GetValue<string>("bundleId");
        projectId            = manifest.GetValue<string>("projectId");
        var cloudBuildTargetName = manifest.GetValue<string>("cloudBuildTargetName");
        Enum.TryParse(cloudBuildTargetName, typeof(BuildTarget), out var target);
        buildTarget = target;

        var parameters = CreateCommandParameters();
        
        UnityBuildTool.ExecuteCommands<IUnityPreBuildCommand>(x=> 
            x.Execute(buildTarget,parameters.arguments,parameters.parameters));
    }

    public static void PostExport(string exportPath) {

        if (string.IsNullOrEmpty(projectId)) {
            Debug.LogError("Project ID is EMPTY PreExport methods can be skiped");
        }
        
        var parameters = CreateCommandParameters();
        
        UnityBuildTool.ExecuteCommands<IUnityPostBuildCommand>(x=> 
            x.Execute(buildTarget,parameters.arguments,parameters.parameters));
        
    }

    private static (IArgumentsProvider arguments,IBuildParameters parameters) CreateCommandParameters()
    {
        
        var argumentsProvider = new ArgumentsProvider(Environment.GetCommandLineArgs());
        Debug.Log(argumentsProvider);
        
        var consolePatameters = UnityBuildTool.ParseBuildArguments(buildTarget, argumentsProvider);
        consolePatameters.buildNumber = buildNumber;
        consolePatameters.buildTarget = buildTarget;
        consolePatameters.projectId   = projectId;
        consolePatameters.bundleId    = bundleId;

        return (argumentsProvider,consolePatameters);
    }
    
}

#endif