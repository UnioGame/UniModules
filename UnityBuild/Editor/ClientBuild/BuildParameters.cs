using System;
using UnityEditor;

namespace Build
{
    [Serializable]
    public class BuildParameters : IBuildParameters {

        public BuildTarget buildTarget;

        public string projectId = string.Empty;
        public int buildNumber = 0;
        public string outputFolder = "Build";
        public string outputFile = "artifact";
        public BuildOptions buildOptions;
        public string bundleId = string.Empty;
            
        //Android
        public string keyStorePath;
        public string keyStorePass;
        public string keyStoreAlias;
        public string keyStoreAliasPass;

        public BuildTarget BuildTarget => this.buildTarget;
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
        
    }
}