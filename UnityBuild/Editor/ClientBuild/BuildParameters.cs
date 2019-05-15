namespace UniGreenModules.UnityBuild.Editor.ClientBuild
{
    using System;
    using Interfaces;
    using UnityEditor;

    [Serializable]
    public class BuildParameters : IBuildParameters {

        public BuildTarget buildTarget;
        public BuildTargetGroup buildTargetGroup;

        public string projectId = string.Empty;
        public int buildNumber = 0;
        public string outputFolder = "Build";
        public string outputFile = "artifact";
        public BuildOptions buildOptions = BuildOptions.None;

        public string bundleId = string.Empty;
            
        //Android
        public string keyStorePath;
        public string keyStorePass;
        public string keyStoreAlias;
        public string keyStoreAliasPass;

        public BuildTarget BuildTarget => this.buildTarget;

        public BuildTargetGroup BuildTargetGroup => buildTargetGroup;

        public int BuildNumber => this.buildNumber;
        public string OutputFile => this.outputFile;
        public string OutputFolder => this.outputFolder;       
        public BuildOptions BuildOptions => this.buildOptions;
        public string ProjectId => projectId;
        public string BundleId => bundleId;

        //android properties
        public string KeyStorePath => this.keyStorePath;
        public string KeyStorePass => this.keyStorePass;
        public string KeyStoreAlias => this.keyStoreAlias;
        public string KeyStoreAliasPass => this.keyStoreAliasPass;

        public void SetBuildOptions(BuildOptions targetBuildOptions, bool replace = true)
        {
            
            if (replace) {
                buildOptions = targetBuildOptions;
                return;
            }

            buildOptions |= targetBuildOptions;
            
        }
        
    }
}