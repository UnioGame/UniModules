namespace UniModules.UniGame.UnityBuild.Editor.ClientBuild.BuildConfiguration
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;

    [Serializable]
    public class UniBuildConfigurationData
    {

        public string ArtifactName = string.Empty;
        
        public BuildTarget BuildTarget;
        
        public BuildTargetGroup BuildTargetGroup;

        public bool CloudBuild = false;
        
    }
}