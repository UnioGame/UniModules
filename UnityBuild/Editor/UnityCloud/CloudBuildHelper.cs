using UnityEngine;
using UnityEditor;
using System;
using UniGreenModules.UnityBuild.Editor.ClientBuild;
using UniGreenModules.UnityBuild.Editor.ClientBuild.BuildConfiguration;
using UniGreenModules.UnityBuild.Editor.ClientBuild.Commands;
using UniGreenModules.UnityBuild.Editor.ClientBuild.Extensions;
using UniGreenModules.UnityBuild.Editor.ClientBuild.Interfaces;
using UniModule.UnityTools.EditorTools;

//https://docs.unity3d.com/Manual/UnityCloudBuildManifestAsScriptableObject.html
//https://docs.unity3d.com/Manual/UnityCloudBuildManifest.html
public class CloudBuildHelper : MonoBehaviour
{
    private static int buildNumber;
    private static string bundleId;
    private static string projectId;
    private static BuildTarget buildTarget;
 
 
#if UNITY_CLOUD_BUILD
   
    public static void PreExport(UnityEngine.CloudBuild.BuildManifestObject manifest) {

#else 

public class DummyManifest
{
    public T GetValue<T>() => default;
    public T GetValue<T>(string key) => default;
}

    public static void PreExport(DummyManifest manifest)
    {
               
#endif   
        buildNumber          = manifest.GetValue<int>("buildNumber");
        bundleId             = manifest.GetValue<string>("bundleId");
        projectId            = manifest.GetValue<string>("projectId");
        var cloudBuildTargetName = manifest.GetValue<string>("cloudBuildTargetName");
        
        Enum.TryParse<BuildTarget>(cloudBuildTargetName, true, out var target);
        
        buildTarget = target;
        

        var parameters = CreateCommandParameters(buildTarget);

        var builder = new UnityPlayerBuilder();
        
        builder.ExecuteCommands<UnityPreBuildCommand>(x=> x.Execute(parameters));
        
    }

    public static void PostExport(string exportPath) {

        if (string.IsNullOrEmpty(projectId)) {
            Debug.LogError("Project ID is EMPTY PreExport methods can be skiped");
        }
        
        var parameters = CreateCommandParameters(EditorUserBuildSettings.activeBuildTarget);
        var builder = new UnityPlayerBuilder();
        
        builder.ExecuteCommands<UnityPostBuildCommand>(x=> x.Execute(parameters,null));
        
    }

    private static IUniBuilderConfiguration CreateCommandParameters(BuildTarget buildTarget)
    {
        
        var argumentsProvider = new ArgumentsProvider(Environment.GetCommandLineArgs());
        
        Debug.Log(argumentsProvider);
        
        var buildParameters = new BuildParameters(buildTarget, argumentsProvider.GetBuildTargetGroup(), argumentsProvider);
        
        buildParameters.buildNumber = buildNumber;
        buildParameters.buildTarget = buildTarget;
        buildParameters.projectId   = projectId;
        buildParameters.bundleId    = bundleId;

        var result = new EditorBuildConfiguration(argumentsProvider,buildParameters);
        return result;
    }

    
}